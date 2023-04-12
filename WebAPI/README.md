# Endpoints

- ## `/api/rating`

  - ### Queries

    - `storeId`

        The store ID of the Microsoft Store app.

    - `market` (optional)

        The default value is `US`.

  - ### Response

    - Success:

      ```json
      {
        "label": "rating",
        "message": "4.7/5 (353.2k)",
        "color": "brightgreen",
        "isError": false,
        "schemaVersion": 1
      }
      ```

    - Error:

      ```json
      {
        "label": "Error",
        "message": "You need to specify a \"storeId\" in query",
        "color": "red",
        "isError": true,
        "schemaVersion": 1
      }
      ```