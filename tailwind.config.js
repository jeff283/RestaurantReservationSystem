/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./Views/**/*.{cshtml,html}", "./Areas/**/*.{cshtml,html}"],
  theme: {
    extend: {},
    colors: {
      "primary-color": "#E6D7B8",
      "secondary-color": "#FFEECA",
      "tertiary-color": "#C0462F",
      "primary-light": "#FFFFFF",
      "primary-dark": "#101010",
      "secondary-dark": "#000000",
    },
  },
  plugins: [],
};
