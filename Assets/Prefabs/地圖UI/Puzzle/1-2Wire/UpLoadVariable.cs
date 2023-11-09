using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;
using System.Globalization;
using System;

public class UpLoadVariable : MonoBehaviour
{
    [Tooltip("The script asset to play.")]
    [ResourcePopup(ScriptsConfiguration.DefaultPathPrefix, ScriptsConfiguration.DefaultPathPrefix, emptyOption: "None (play script text)")]
    [SerializeField] private string scriptName = default;
    [TextArea(3, 10), Tooltip("The naninovel script text (commands) to execute; has no effect when `Script Name` is specified. Argument of the event (if any) can be referenced in the script text via `{arg}` expression. Conditional block commands (if, else, etc) are not supported.")]
    [SerializeField] private string scriptText = default;
    [Tooltip("Whether to automatically play the script when the game object is instantiated.")]
    [SerializeField] private bool playOnAwake = false;
    [Tooltip("Whether to disable waiting for input mode when the script is played.")]
    [SerializeField] private bool disableWaitInput = false;
    public GameObject P1, P2, P3;
    GameObject F1, F2, F3;
    bool Determination;
    int gg=0;
    private string argument;

    public void Play()
    {
        argument = null;
        PlayScriptAsync();
    }

    public void Play(string argument)
    {
        this.argument = argument;
        PlayScriptAsync();
    }

    public void Play(float argument)
    {
        this.argument = argument.ToString(CultureInfo.InvariantCulture);
        PlayScriptAsync();
    }

    public void Play(int argument)
    {
        this.argument = argument.ToString(CultureInfo.InvariantCulture);
        PlayScriptAsync();
    }

    public void Play(bool argument)
    {
        this.argument = argument.ToString(CultureInfo.InvariantCulture).ToLower();
        PlayScriptAsync();
    }

    private void Awake()
    {
        if (playOnAwake) Play();
    }

    private async void PlayScriptAsync()
    {
        if (!string.IsNullOrEmpty(scriptName))
        {
            var player = Engine.GetService<IScriptPlayer>();
            if (player is null) throw new InvalidOperationException($"Failed to play a script via `{gameObject.name}` game object: player service is not available. Make sure the engine is initialized.");
            await player.PreloadAndPlayAsync(scriptName);
            return;
        }

        if (disableWaitInput) Engine.GetService<IScriptPlayer>().SetWaitingForInputEnabled(false);

        if (!string.IsNullOrWhiteSpace(scriptText))
        {
            var text = string.IsNullOrEmpty(argument) ? scriptText : scriptText.Replace("{arg}", argument);
            var script = Script.FromScriptText($"`{name}` generated script", text);
            var playlist = new ScriptPlaylist(script);
            await playlist.ExecuteAsync();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gg = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //var variableManager = Engine.GetService<ICustomVariableManager>();
        //var myValue = variableManager.GetVariableValue("");
        //myValue += "Hello!";
        //variableManager.SetVariableValue("MyVariable", myValue);
        F1 = P1.transform.parent.gameObject;
        F2 = P2.transform.parent.gameObject;
        F3 = P3.transform.parent.gameObject;
        Determination = (F1==GameObject.Find("A")) & (F2 == GameObject.Find("B")) & (F3 == GameObject.Find("C"));


        if ((gg==0)&&Determination)
        {
            gg += 1;
            Play();
        }
    }
}
