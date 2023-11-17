using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;
using System.Globalization;
using System;

public class UnbrellaUse : MonoBehaviour
{
    bool UnbrellaUseed;
    [Tooltip("The script asset to play.")]
    [ResourcePopup(ScriptsConfiguration.DefaultPathPrefix, ScriptsConfiguration.DefaultPathPrefix, emptyOption: "None (play script text)")]
    [SerializeField] private string scriptName = default;
    [TextArea(3, 10), Tooltip("The naninovel script text (commands) to execute; has no effect when `Script Name` is specified. Argument of the event (if any) can be referenced in the script text via `{arg}` expression. Conditional block commands (if, else, etc) are not supported.")]
    [SerializeField] private string scriptText1 = default;
    [TextArea(3, 10), Tooltip("The naninovel script text (commands) to execute; has no effect when `Script Name` is specified. Argument of the event (if any) can be referenced in the script text via `{arg}` expression. Conditional block commands (if, else, etc) are not supported.")]
    [SerializeField] private string scriptText2 = default;
    [Tooltip("Whether to automatically play the script when the game object is instantiated.")]
    [SerializeField] private bool playOnAwake = false;
    [Tooltip("Whether to disable waiting for input mode when the script is played.")]
    [SerializeField] private bool disableWaitInput = false;
    // Start is called before the first frame update
    void Start()
    {


        //variableManager.TrySetVariableValue("MyBooleanVariableName", boolValue);
    }

    // Update is called once per frame
    void Update()
    {
        var variableManager = Engine.GetService<ICustomVariableManager>();
        variableManager.TryGetVariableValue<bool>("AllowUnbrellaUsing", out var boolValue);
        UnbrellaUseed = boolValue;
        //print(boolValue);
    }
    public void UseUnbrella()
    {
        if(UnbrellaUseed)
        {
            Play1();
        }
        if (UnbrellaUseed==false)
        {
            Play2();
        }
    }
    private string argument;

    public void Play1()
    {
        argument = null;
        PlayScriptAsync1();
        print(1);
    }
    public void Play2()
    {
        argument = null;
        PlayScriptAsync2();
    }
    private void Awake()
    {
        if (playOnAwake) Play1();
    }

    private async void PlayScriptAsync1()
    {
        if (!string.IsNullOrEmpty(scriptName))
        {
            var player = Engine.GetService<IScriptPlayer>();
            if (player is null) throw new InvalidOperationException($"Failed to play a script via `{gameObject.name}` game object: player service is not available. Make sure the engine is initialized.");
            await player.PreloadAndPlayAsync(scriptName);
            return;
        }

        if (disableWaitInput) Engine.GetService<IScriptPlayer>().SetWaitingForInputEnabled(false);

        if (!string.IsNullOrWhiteSpace(scriptText1))
        {
            var text = string.IsNullOrEmpty(argument) ? scriptText1 : scriptText1.Replace("{arg}", argument);
            var script = Script.FromScriptText($"`{name}` generated script", text);
            var playlist = new ScriptPlaylist(script);
            await playlist.ExecuteAsync();
        }
    }
    private async void PlayScriptAsync2()
    {
        if (!string.IsNullOrEmpty(scriptName))
        {
            var player = Engine.GetService<IScriptPlayer>();
            if (player is null) throw new InvalidOperationException($"Failed to play a script via `{gameObject.name}` game object: player service is not available. Make sure the engine is initialized.");
            await player.PreloadAndPlayAsync(scriptName);
            return;
        }

        if (disableWaitInput) Engine.GetService<IScriptPlayer>().SetWaitingForInputEnabled(false);

        if (!string.IsNullOrWhiteSpace(scriptText2))
        {
            var text = string.IsNullOrEmpty(argument) ? scriptText2 : scriptText2.Replace("{arg}", argument);
            var script = Script.FromScriptText($"`{name}` generated script", text);
            var playlist = new ScriptPlaylist(script);
            await playlist.ExecuteAsync();
        }
    }
}
