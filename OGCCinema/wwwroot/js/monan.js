// wwwroot/js/monan.js xử lý giỏ hàng
document.addEventListener('DOMContentLoaded', () => {
    // Cuộn mượt mà đến anchor
    document.querySelectorAll('a[href*="#"]').forEach(anchor => {
        anchor.addEventListener('click', (e) => {
            e.preventDefault();
            const targetId = anchor.getAttribute('href').split('#')[1];
            const target = document.getElementById(targetId);
            if (target) {
                target.scrollIntoView({ behavior: 'smooth' });
            }
        });
    });

    let cart = JSON.parse(localStorage.getItem('cart')) || [];

    function updateCartTotal() {
        const total = cart.reduce((sum, item) => sum + item.price * item.quantity, 0);
        document.getElementById('cart-total').textContent = `Tổng tiền: ${total.toLocaleString()} VNĐ`;
    }

    document.querySelectorAll('.add-to-cart').forEach(button => {
        button.addEventListener('click', () => {
            const id = button.getAttribute('data-id');
            const name = button.getAttribute('data-name');
            const price = parseFloat(button.getAttribute('data-price'));

            const item = cart.find(i => i.id === id);
            if (item) {
                item.quantity += 1;
            } else {
                cart.push({ id, name, price, quantity: 1 });
            }

            localStorage.setItem('cart', JSON.stringify(cart));
            updateCartTotal();
        });
    });

    updateCartTotal();
});