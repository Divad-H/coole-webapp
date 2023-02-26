const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      "/WeatherForecast",
      "/connect/token",
      "/Registration",
      "/registration",
      "/AdminProducts",
      "/adminproducts",
      "/Shop",
      "/shop",
      "/UserAccount",
      "/useraccount",
      "/Dashboard",
      "/dashboard",
      "/UserSettings",
      "/userSettings"
    ],
    target: "https://localhost:7038",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
