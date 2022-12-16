# WTF is this thing?

The FirebotProxy is an app that runs in tandem with Firebot. It provides you with additional functionality which, either cannot be configured in Firebot by itself, or involves jumping through lots of hoops to get it working.

The FirebotProxy eliminates this dependency to make these commands easier to setup and run "out of the box".

# Getting Started

## Basic Installation

Head over to the releases section on the right and take a look at the latest version of the proxy (recommended), or alternatively, dive into any of the older versions available.

You'll want to grab the relevant archive file for OS you're currently using (Windows, Mac or Linux). Once downloaded, unpack the files into a folder of your choosing and run the executable.

On startup, it will create a cache file in your home folder called `firebotproxy.db` which is used by the app to perform the various operations explained under the [Using the FirebotProxy](#using-the-firebotproxy) section.

## Uninstalling

If you want to uninstall the proxy, delete the folder you created for the app, and delete the `firebotproxy.db` file in your home folder. That's it!

## Building from Source

If you're feeling adventurous, you're more than welcome to build and run the proxy solution from the source code in this repository!

### Prerequisites

- You have a Git client installed, such as:
  - The generic git CLI tool (for powershell, bash, zsh, etc.)
  - The GitHub CLI tool.
  - GitHub Desktop.
- You have the dotnet CLI installed
  - Must be version `6.0.x` or better.

### Build and Run the Solution

1. Clone this repository from GitHub and navigate to the source folder:
```shell
git clone https://github.com/blackb0x3/FirebotProxy.git ~/FirebotProxy

cd ~/FirebotProxy
```
2. Run the restore command on the solution to download and install any missing dependencies:
```shell
dotnet restore
```
3. Run the build command on the solution to compile the application
```shell
dotnet build
```
4. Run the Api and BackgroundWorker projects to get the full solution going:
```shell
dotnet run --project FirebotProxy.Api/FirebotProxy.Api.csproj

dotnet run --project FirebotProxy.BackgroundWorker/FirebotProxy.BackgroundWorker.csproj
```

# Using the FirebotProxy

Commands and Events in the FirebotProxy are triggered by HTTP requests sent locally by Firebot itself. These requests point to http://localhost:7296 with routes for `/command`s and `/event`s, respectively.

You can activate a FirebotProxy command or event trigger, by adding a HTTP request effect to a command.

All requests sent to the proxy should contain the header `Content-Type: application/json` to ensure that the proxy can parse data that is sent to it, in the expected manner.

## Commands

### Get Chat Rank

Analyses the cached chat messages and ranks the provided viewer based on the number of messages they have sent.

**URL**: `https://localhost:7296/Commands/ChatRank/<viewer username>`

**METHOD**: GET

**RESPONSE**:

```json
{
  "messageCount": 12345,
  "rankPosition": "1st" // alternatively: 2nd, 3rd, 4th etc.
}
```

### Get Chat Plot

Analyses the cached chat messages for a viewer, and creates a picture chart, showing the chat message count over time (requires **at least 2 days worth of posts** to make the request valid).

**URL**: `https://localhost:7296/Commands/ChatPlot/<viewer username>`

**METHOD**: GET

**RESPONSE**:

```json
{
  "chartUrl": "https://quickchart.io/chart/render/<some-guid-of-a-chart>"
}
```

### Get Chat Word Cloud

Analyses the cached chat messages for a viewer, or a particular stream, or both, and creates a word cloud of the most used words / phrases based on the query.

**URL**: `https://localhost:7296/Commands/WordCloud`

**METHOD**: POST

**BODY**:

```json
{
  "viewerUsername": "blackb0x3",
  "streamDate": "2022-11-18",
  "wordCloudSettings": {
    "width": 1000,
    "height": 1000,
    "backgroundHexColour": "#fedbca",
    "fontFamily": "ubuntu",
    "wordHexColours": [
      "#111111",
      "#222222",
      "#333333",
      "#444444",
      "#555555",
      "#666666"
    ]
  }
}
```

**RESPONSE**:

```json
{
  "wordCloudUrl": "<word-cloud-url>"
}
```

## Events

### Chat Message Caching

The proxy supports the caching of all chat messages received for a maximum of 30 days. After which, they will automatically be removed from your machine.

These chat messages are used by the [Chat Rank](#get-chat-rank) and [Chat Plot](#get-chat-plot) features of the proxy.

**EXAMPLE TRIGGER ON**: Chat Message

**URL**: `https://localhost:7296/Events/LogChatMessage`

**METHOD**: POST

**BODY**:

```json
{
  "content": "$chatMessage",
  "timestamp": "$date[YYYY-MM-DD] $time[HH:mm:ss.SSSSSSSSS]",
  "senderUsername": "$user"
}
```

### Remove Cached Messages of Specific Viewer

Should the need arise, the proxy supports the deletion of cached chat messages when provided a specific viewer name.

**EXAMPLE TRIGGER ON**: Viewer Banned

**URL**: `https://localhost:7296/Events/RemoveViewerMessages`

**METHOD**: POST

**BODY**:

```json
{
  "viewerUsername": "$user"
}
```

# FAQ
I guess I'll put some questions and answers in here if I get asked a lot of stuff.
