using System;
using System.Collections.Generic;

[Serializable]
public class ResponseChannels
{
    public string voice;
}
[Serializable]
public class Root
{
    public string placeholder;
    public ResponseChannels response_channels;
}

