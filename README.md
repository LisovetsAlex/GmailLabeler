## Email labeler

Add appsettings.json with:
```
{
  "Settings": {
    "OpenAiApiKey": "",
    "OpenAiProjKey": "",
    "OpenAiOrgKey": "",
    "ImapAddress": "",    // gmail address
    "ImapPass": "",       // gmail app passkey
    "Categories": [
      {
        "Name": "Gaming News",
        "Description": "All mail that is about games and news about them."
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
