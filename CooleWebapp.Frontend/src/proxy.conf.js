const PROXY_CONFIG = [
  {
    context: [
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
      "/userSettings",
      "/Fridge",
      "/fridge",
      "/Statistics",
      "/statistics"
    ],
    target: "https://localhost:7038",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
