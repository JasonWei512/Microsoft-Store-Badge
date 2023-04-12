# Try it out!

👉 **[Microsoft Store Badge Generator](https://JasonWei512.github.io/Microsoft-Store-Badge)**

# Introduction

A web app to generate a badge with rating for your Microsoft Store app.

Add the badge to your GitHub README or website.

The rating on badge will be updated every 12 hours.

# Examples

[![](https://img.shields.io/endpoint?url=https%3A%2F%2Fmicrosoft-store-badge.fly.dev%2Fapi%2Frating%3FstoreId%3D9NF7JTB3B17P%26market%3DUS&style=flat&color=brightgreen)](https://www.microsoft.com/store/productId/9NF7JTB3B17P)

[![Ambie](https://img.shields.io/endpoint?url=https%3A%2F%2Fmicrosoft-store-badge.fly.dev%2Fapi%2Frating%3FstoreId%3D9P07XNM5CHP0%26market%3DUS&style=flat-square&label=Ambie&color=orange&logo=Windows)](https://www.microsoft.com/store/productId/9P07XNM5CHP0)

[![NanaZip](https://img.shields.io/endpoint?url=https%3A%2F%2Fmicrosoft-store-badge.fly.dev%2Fapi%2Frating%3FstoreId%3D9N8G7TSCL18R%26market%3DUS&style=social&label=NanaZip&color=brightgreen&logo=)](https://www.microsoft.com/store/productId/9N8G7TSCL18R)

[![Hi-Fi Rush](https://img.shields.io/endpoint?url=https%3A%2F%2Fmicrosoft-store-badge.fly.dev%2Fapi%2Frating%3FstoreId%3D9NFTC552K3GJ%26market%3DUS&style=for-the-badge&label=Hi-Fi+Rush&color=brightgreen&logo=Xbox)](https://www.microsoft.com/store/productId/9NFTC552K3GJ)

# How does it work

- The [WebAPI](./WebAPI/) folder:

  - An ASP.NET Core web API providing app ratings

  - Uses [StoreLib](https://github.com/StoreDev/StoreLib) to get app rating

  - The API response is consumed by [Shields.IO](https://shields.io) to generate an [endpoint badge](https://shields.io/endpoint)

  - Code on `main` branch will be deployed to [Fly.io](https://fly.io)

- The [WebUI](./WebUI/) folder:

  - A Vue web app to help you generate badges

  - Code on `main` branch will be deployed to GitHub Pages

# Known issues

Due to [StoreLib](https://github.com/StoreDev/StoreLib)'s limitation, generating badges for unpackaged Win32 apps (like [PowerToys](https://apps.microsoft.com/store/detail/XP89DCGQ3K6VLD) and [VSCode](https://apps.microsoft.com/store/detail/XP9KHM4BK9FZ7Q)) is not supported.

# Acknowledgements

- Inspired by [infinitepower18/msstore-shields](https://github.com/infinitepower18/msstore-shields)