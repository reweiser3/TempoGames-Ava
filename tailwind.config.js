/** @type {import('tailwindcss').Config} */
module.exports = {
    darkMode: 'class',
    content: [
        './Pages/**/*.razor',
        './Components/**/*.razor',
        './wwwroot/index.html',
        './**/*.cshtml',
        './node_modules/flowbite/**/*.js'
    ],
    theme: {
        extend: {
            colors: {
                primary: {
                    "50": "#e0f4f7",
                    "100": "#b3e4e9",
                    "200": "#80d1db",
                    "300": "#4dbccf",
                    "400": "#26a8c0",
                    "500": "#0f7278",  // Main primary color
                    "600": "#0e6469",
                    "700": "#0d5457",
                    "800": "#0b4445",
                    "900": "#093637",
                    "950": "#052323"
                },
                secondary: {
                    "50": "#f2f2f2",
                    "100": "#e6e6e6",
                    "200": "#cccccc",
                    "300": "#b3b3b3",
                    "400": "#999999",
                    "500": "#4e4f52",  // Additional colors
                    "600": "#444446",
                    "700": "#3b3b3c",
                    "800": "#313131",
                    "900": "#292929"
                },
                accent: {
                    "50": "#f9fafa",
                    "100": "#f3f5f6",
                    "200": "#e5eaec",
                    "300": "#d7dfe3",
                    "400": "#c9d4d9",
                    "500": "#a2a9ad",  // Additional colors
                    "600": "#91999d",
                    "700": "#7f8689",
                    "800": "#6e7376",
                    "900": "#5d6062"
                },
                neutral: {
                    "50": "#fafafa",
                    "100": "#f5f5f5",
                    "200": "#eeeeee",
                    "300": "#e0e0e0",
                    "400": "#bdbdbd",
                    "500": "#9e9e9e",
                    "600": "#757575",
                    "700": "#616161",
                    "800": "#424242",
                    "900": "#212121"
                },
                background: {
                    "light": "#ffffff",  // White color
                    "dark": "#050000"  // Black color
                },
                surface: {
                    "light": "#f7f7f7",  // Light gray surface
                    "dark": "#1f2125"  // Dark surface color
                },
                border: {
                    "default": "#222529"  // Border color
                }
            },
            spacing: {
                '8': '2rem',    // 32px
                '9': '2.5rem',  // 40px
                '10': '3rem',   // 48px
                '11': '3.5rem', // 56px
                '12': '4.5rem', // 72px
                '13': '5rem',   // 80px
                '14': '5.5rem', // 88px
                '15': '6rem',   // 96px
                '16': '6.5rem', // 104px
            },
            fontFamily: {
                'body': [
                    'Inter',
                    'ui-sans-serif',
                    'system-ui',
                    '-apple-system',
                    'system-ui',
                    'Segoe UI',
                    'Roboto',
                    'Helvetica Neue',
                    'Arial',
                    'Noto Sans',
                    'sans-serif',
                    'Apple Color Emoji',
                    'Segoe UI Emoji',
                    'Segoe UI Symbol',
                    'Noto Color Emoji'
                ],
                'sans': [
                    'Inter',
                    'ui-sans-serif',
                    'system-ui',
                    '-apple-system',
                    'system-ui',
                    'Segoe UI',
                    'Roboto',
                    'Helvetica Neue',
                    'Arial',
                    'Noto Sans',
                    'sans-serif',
                    'Apple Color Emoji',
                    'Segoe UI Emoji',
                    'Segoe UI Symbol',
                    'Noto Color Emoji'
                ]
            },
            padding: {
                '1': '0.25rem',
                '2': '0.5rem',
                '3': '0.75rem',
                '4': '1rem',
                '5': '1.25rem',
                '6': '1.5rem',
                '8': '2rem'
            },
        }
    },
    plugins: [
        require('flowbite/plugin')
    ]
}
