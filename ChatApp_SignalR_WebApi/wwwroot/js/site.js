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

        // Очистка предыдущих сообщений об ошибках
        document.querySelectorAll('.validation-message').forEach(span => span.textContent = '');

        // Получение значений полей
        const signinUsername = document.getElementById('signin-username').value.trim();
        const signinPassword = document.getElementById('signin-password').value.trim();
        const signinConfirmation = document.getElementById('signin-confirmation').value.trim();

        // Проверка поля имени пользователя
        if (signinUsername === '') {
            document.getElementById('signin-username-error').textContent = 'Username is required';
            isValid = false;
        }

        // Проверка пароля
        if (signinPassword === '') {
            document.getElementById('signin-password-error').textContent = 'Password is required';
            isValid = false;
        } else if (signinPassword.length < 6) {
            document.getElementById('signin-password-error').textContent = 'Password must be at least 6 characters long';
            isValid = false;
        }

        // Проверка подтверждения пароля
        if (signinConfirmation !== signinPassword) {
            document.getElementById('signin-confirmation-error').textContent = 'Passwords do not match';
            isValid = false;
        }

        // Если валидация не пройдена, прерываем отправку формы
        if (!isValid) {
            return;
        }

        // Если валидация пройдена, отправляем форму
        const user = {
            userName: signinUsername,
            password: signinPassword,
            passwordHash: '' // Используйте bcrypt для генерации хеша пароля на стороне сервера
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
                alert(data.message); // Сообщение об успешной регистрации
                document.getElementById('signin-modal').style.display = 'none'; // Закрытие модального окна
            })
            .catch(error => {
                console.error('Error:', error);
            });
    });
});