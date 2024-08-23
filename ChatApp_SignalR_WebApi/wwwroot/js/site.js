document.addEventListener('DOMContentLoaded', function () {

    const greetingLogoutContainer = document.getElementById('greeting-logout-container');
    const loginSigninContainer = document.getElementById('login-signin-container');
    const loginModal = document.getElementById('login-modal');
    const signinModal = document.getElementById('signin-modal');
    const inputMessage = document.getElementById('input-message');
    const sendMessage = document.getElementById('send-message');
    const usersContent = document.getElementById('users-content');
    const chatContent = document.getElementById('chat-content');

    greetingLogoutContainer.style.display = 'none';
    loginSigninContainer.style.display = '';
    loginModal.style.display = 'none';
    signinModal.style.display = 'none';
    inputMessage.disabled = true;
    sendMessage.disabled = true;

    document.getElementById('open-login-modal').addEventListener('click', () => {
        loginModal.style.display = '';
    });

    document.getElementById('open-signin-modal').addEventListener('click', () => {
        signinModal.style.display = '';
    });

    document.getElementById('login-close').addEventListener('click', () => {
        loginModal.style.display = 'none';
    });

    document.getElementById('signin-close').addEventListener('click', () => {
        signinModal.style.display = 'none';
    });

    // ------------------------------------HUB CONNECTION-----------------------------------

    const hubConnection = new signalR.HubConnectionBuilder().withUrl("/chat").build(); 

    // ------------------------------------REGISTRATION-------------------------------------

    document.getElementById('signin-submit').addEventListener('click', function (event) {
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

        if (!isValid) return;

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

    // ------------------------------------LOGIN-------------------------------------
    document.getElementById('login-submit').addEventListener('click', async function (event) {
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

        if (!isValid) return;

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

    //----------------------------------HUB CALLBACKS---------------------------
    async function connectToChatHub(username) {
        hubConnection.on("Connected", function (connectionId, username, allUsers) {
            document.getElementById('session-id').value = connectionId;
            document.getElementById('username').value = username;
            usersContent.innerHTML = '';
            allUsers.forEach(user => {
                const newUser = document.createElement('p');
                newUser.textContent = user.username;
                newUser.classList.add('user');
                usersContent.appendChild(newUser);
            });

            const userConnectedMessage = document.createElement('p');
            userConnectedMessage.textContent = `${username} joined chat`;
            userConnectedMessage.classList.add('message');
            chatContent.appendChild(userConnectedMessage);
        });

        hubConnection.on("NewUserConnected", function (connectionId, username) {

            const newUser = document.createElement('p');
            newUser.textContent = username;
            newUser.classList.add('user');
            usersContent.appendChild(newUser);

            const userConnectedMessage = document.createElement('p');
            userConnectedMessage.textContent = `${username} joined chat`;
            userConnectedMessage.classList.add('message');
            chatContent.appendChild(userConnectedMessage);
        });

        hubConnection.on("AddMessage", function (username, message, timestamp) {

            const newMessage = document.createElement('p');
            newMessage.classList.add('message');
            newMessage.textContent = `${username}: ${message} ${timestamp}`;
            chatContent.appendChild(newMessage);
        });

        hubConnection.on("UserDisconnected", function (connectionId, username) {

            const userDisconnectedMessage = document.createElement('p');
            userDisconnectedMessage.textContent = `${username} left the chat`;
            userDisconnectedMessage.classList.add('message');
            chatContent.appendChild(userDisconnectedMessage);

            const userElements = document.querySelectorAll('.user');
            userElements.forEach(userElement => {
                if (userElement.textContent === username) {
                    userElement.remove();
                }
            });
        });

        try {
            await hubConnection.start();
            await hubConnection.invoke("Connect", username);
        } catch (error) {
            console.error('Error starting connection:', error);
        }
    }

    // ---------------------------MESSAGES SENDING-----------------
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
            hubConnection.invoke("Send", username, message)
                .then(() => {
                    inputMessage.value = '';
                })
                .catch(error => console.error('Error sending message:', error));
        }
    }

    // -----------------------------LOG OUT-------------------------------------------

    document.getElementById('logout-button').addEventListener('click', async () => {
        try {
            await hubConnection.invoke("Disconnect");

            greetingLogoutContainer.style.display = 'none';
            loginSigninContainer.style.display = '';
            inputMessage.disabled = true;
            sendMessage.disabled = true;
            sendMessage.classList.remove('button');
            sendMessage.classList.add('button-disabled');

            await hubConnection.stop();
            console.log('Disconnected successfully');
        } catch (err) {
            console.error('Error disconnecting:', err);
        }
    });

    //------------------------------------CHAT HISTRY-------------------------------------
    async function loadChatHistory() {
        try {
            const response = await fetch('/api/chat/history');
            if (!response.ok) {
                throw new Error('Network response error');
            }
            const messages = await response.json();
            messages.forEach(message => {
                const messageElement = document.createElement('p');
                messageElement.classList.add('message');
                messageElement.textContent = `${message.userName}: ${message.message} ${message.timestamp}`;
                chatContent.appendChild(messageElement);
            });
        } catch (error) {
            console.error('Error loading chat history:', error);
        }
    }
    loadChatHistory();
});
