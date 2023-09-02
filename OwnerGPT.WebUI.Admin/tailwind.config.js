/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ['./Views/**/*.cshtml'],
    darkMode: false,
    mode: 'jit',
    theme: {
        extend: {},
    },
    plugins: [
        require('flowbite/plugin')
    ]
}

