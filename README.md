## Email labeler

Add appsettings.json with:
```
{
  "Settings": {
    "OpenAiApiKey": "",
    "OpenAiProjKey": "",
    "OpenAiOrgKey": "",
    "ImapAddress": "",    // gmail address
    "ImapPass": "",       // gmail app password
    "Categories": [
      {
        "Name": "Weather News",
        "Description": "All mail that is about weather and news about it."
      },
      {
        "Name": "Other",
        "Description": "Everything else."
      }...
    ]
  }
}
```
Run the program and it will categorize your gmail emails with labels.
Uses chatgpt to categorize email.
