document.addEventListener('DOMContentLoaded', function () {

    const greetingLogoutContainer = document.getElementById('greeting-logout-container');
    greetingLogoutContainer.style.display = 'none';

    const loginSigninContainer = document.getElementById('login-signin-container');
    loginSigninContainer.style.display = ''

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

    const loginClose = document.getElementById('login-close');
    loginClose.addEventListener('click', function () {
        loginModal.style.display = 'none';
    });

    const signinClose = document.getElementById('signin-close');
    signinClose.addEventListener('click', function () {
        signinModal.style.display = 'none';
    });

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
    loginSubmit.addEventListener('click', function (event) {
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

        const loginData = {
            UserName: loginUsername,
            Password: loginPassword
        };

        fetch('api/account/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(loginData)
        })
            .then(response => {
                if (!response.ok) {
                    return response.json().catch(() => {
                        throw new Error('Invalid username or password');
                    });
                }
                return response.json();
            })
            .then(data => {
                alert(data.message);
                loginModal.style.display = 'none';
                loginSigninContainer.style.display = 'none';
                greetingLogoutContainer.style.display = '';
            })
            .catch(error => {
                document.getElementById('login-password-error').textContent = error.message;
            });
    });

});