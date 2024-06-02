
        document.addEventListener('DOMContentLoaded', (event) => {
            const menuButton = document.getElementById('user-menu-button');
    const userMenu = document.getElementById('user-menu');

            menuButton.addEventListener('click', () => {
                const isExpanded = menuButton.getAttribute('aria-expanded') === 'true';
    menuButton.setAttribute('aria-expanded', !isExpanded);
    userMenu.classList.toggle('hidden');
            });

            // Close the menu if the user clicks outside of it
            document.addEventListener('click', (event) => {
                if (!menuButton.contains(event.target) && !userMenu.contains(event.target)) {
        userMenu.classList.add('hidden');
    menuButton.setAttribute('aria-expanded', 'false');
                }
            });
        });
