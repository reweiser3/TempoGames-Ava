/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './**/*.razor',
        './**/*.cshtml',
        './**/*.html',
        './**/*.cs',
        './node_modules/flowbite/**/*.js'
    ],
    theme: {
        extend: {},
    },
    plugins: [
        require('flowbite/plugin')
    ],
}