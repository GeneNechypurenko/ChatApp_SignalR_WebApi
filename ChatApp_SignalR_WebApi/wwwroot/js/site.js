document.addEventListener('DOMContentLoaded', function () {

    const greetingLogoutContainer = document.getElementById('greeting-logout-container');
    greetingLogoutContainer.style.display = 'none';

    const loginSigninContainer = document.getElementById('login-signin-container');
    loginSigninContainer.style.display = '';

    const loginModal = document.getElementById('login-modal');
    loginModal.style.display = 'none';

    const signinModal = document.getElementById('signin-modal');
    signinModal.style.display = 'none';

    const openLoginModal = document.getElementById('open-login-modal');
    openLoginModal.addEventListener('click', function () {
        loginModal.style.display = '';
    });

    const openSigninModal = document.getElementById('open-signin-modal');
    openSigninModal.addEventListener('click', function () {
        signinModal.style.display = '';
    });

    const logoutButton = document.getElementById('logout-button');

    const loginClose = document.getElementById('login-close');
    loginClose.addEventListener('click', function () {
        loginModal.style.display = 'none';
    });

    const signinClose = document.getElementById('signin-close');
    signinClose.addEventListener('click', function () {
        signinModal.style.display = 'none';
    });

    const usersContent = document.getElementById('users-content');
    const chatContent = document.getElementById('chat-content');

    const inputMessage = document.getElementById('input-message');
    inputMessage.disabled = true;

    const sendMessage = document.getElementById('send-message');
    sendMessage.disabled = true;

    //---------------------------------------  HUB CONNECTION:

    const connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

    //---------------------------------------  USER REGISTRATION:

    const signinSubmit = document.getElementById('signin-submit');
    signinSubmit.addEventListener('click', function (event) {
        event.preventDefault();

        let isValid = true;

        document.querySelectorAll('.validation-message').forEach(span => span.textContent = '');

        const signinUsername = document.getElementById('signin-username').value.trim();
        const signinPassword = document.getElementById('signin-password').value.trim();
        const signinConfirmation = document.getElementById('signin-confirmation').value.trim();

        if (signinUsername === '') {
            document.getElementById('signin-username-error').textContent = 'Username is required';
            isValid = false;
        }

        if (signinPassword === '') {
            document.getElementById('signin-password-error').textContent = 'Password is required';
            isValid = false;
        } else if (signinPassword.length < 6) {
            document.getElementById('signin-password-error').textContent = 'Password must be at least 6 characters long';
            isValid = false;
        }

        if (signinConfirmation !== signinPassword) {
            document.getElementById('signin-confirmation-error').textContent = 'Passwords do not match';
            isValid = false;
        }

        if (!isValid) {
            return;
        }

        const user = {
            userName: signinUsername,
            password: signinPassword,
            passwordHash: ''
        };

        fetch('api/account/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        })
            .then(response => response.json())
            .then(data => {
                alert(data.message);
                signinModal.style.display = 'none';
            })
            .catch(error => {
                console.error('Error:', error);
            });
    });

    //---------------------------------------  USER LOGIN:

    const loginSubmit = document.getElementById('login-submit');
    loginSubmit.addEventListener('click', async function (event) {
        event.preventDefault();

        let isValid = true;

        document.querySelectorAll('.validation-message').forEach(el => el.textContent = '');

        const loginUsername = document.getElementById('login-username').value.trim();
        const loginPassword = document.getElementById('login-password').value.trim();

        if (loginUsername === '') {
            document.getElementById('login-username-error').textContent = 'Username is required';
            isValid = false;
        }

        if (loginPassword === '') {
            document.getElementById('login-password-error').textContent = 'Password is required';
            isValid = false;
        }

        if (!isValid) {
            return;
        }

        try {
            const response = await fetch('/api/account/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ userName: loginUsername, password: loginPassword })
            });

            if (response.ok) {

                const user = await response.json();

                loginModal.style.display = 'none';
                loginSigninContainer.style.display = 'none';
                greetingLogoutContainer.style.display = '';
                document.getElementById('greeting').textContent = 'Welcome, ' + user.userName;

                inputMessage.disabled = false;
                sendMessage.disabled = false;
                sendMessage.classList.remove('button-disabled');
                sendMessage.classList.add('button');

                await connectToChatHub(user.userName);

            } else {
                const error = await response.json();
                document.getElementById('login-password-error').textContent = error.message;
            }
        } catch (error) {
            console.error("Error: ", error);
        }
    });

    async function connectToChatHub(username) {

        connection.on("Connected", (userSessionId, username) => {

            console.log(`User ${username} connected with session ID ${userSessionId}`);

            const newUser = document.createElement('p');
            newUser.textContent = username;
            newUser.classList.add('user');
            usersContent.appendChild(newUser);

            const userConnectedMessage = document.createElement('p');
            userConnectedMessage.textContent = `${username} joined chat`;
            userConnectedMessage.classList.add('message');
            chatContent.appendChild(userConnectedMessage);

            document.getElementById('session-id').value = userSessionId;
            document.getElementById('username').value = username;
        });

        connection.on("NewUserConnected", (connectionId, username) => {

            console.log(`New user connected: ${username} (${connectionId})`);

            const newUser = document.createElement('p');
            newUser.textContent = username;
            newUser.classList.add('user');
            usersContent.appendChild(newUser);

            const userConnectedMessage = document.createElement('p');
            userConnectedMessage.textContent = `${username} joined chat`;
            userConnectedMessage.classList.add('message');
            chatContent.appendChild(userConnectedMessage);
        });

        connection.on("AddMessage", (username, message) => {

            console.log(`Message from ${username}: ${message}`);

            const newMessage = document.createElement('p');
            newMessage.classList.add('message');
            newMessage.textContent = `${username}: ${message}`;
            chatContent.appendChild(newMessage);
        });

        await connection.start();
        await connection.invoke("Connect", username);
    }

    //---------------------------------------  SEND MESSAGE:

    inputMessage.addEventListener('keydown', function (event) {
        if (event.key === 'Enter') {
            event.preventDefault();
            sendChatMessage();
        }
    });

    sendMessage.addEventListener('click', function () {
        sendChatMessage();
    });

    function sendChatMessage() {
        const message = inputMessage.value.trim();
        if (message) {
            const username = document.getElementById('username').value;
            const sessionId = document.getElementById('session-id').value;

            connection.invoke("Send", username, message)
                .then(() => {
                    inputMessage.value = '';
                    console.log(`Message sent: ${message}`);
                })
                .catch(error => console.error('Error sending message:', error));
        }
    } 
});
