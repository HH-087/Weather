using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Globalization;
using Naninovel;
using System;

public class Board : MonoBehaviour
{
    public Ring[] ringList; //由外到內
    [Tooltip("The script asset to play.")]
    [ResourcePopup(ScriptsConfiguration.DefaultPathPrefix, ScriptsConfiguration.DefaultPathPrefix, emptyOption: "None (play script text)")]
    [SerializeField] private string scriptName = default;
    [TextArea(3, 10), Tooltip("The naninovel script text (commands) to execute; has no effect when `Script Name` is specified. Argument of the event (if any) can be referenced in the script text via `{arg}` expression. Conditional block commands (if, else, etc) are not supported.")]
    [SerializeField] private string scriptText = default;
    [Tooltip("Whether to automatically play the script when the game object is instantiated.")]
    [SerializeField] private bool playOnAwake = false;
    [Tooltip("Whether to disable waiting for input mode when the script is played.")]
    [SerializeField] private bool disableWaitInput = false;
    bool Determination;
    private string argument;

    public void Play()
    {
        argument = null;
        PlayScriptAsync();
        print("1234");
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
    public void OnMouseUp()
    {
        CheckRingsAngle();
        //print("qwe");
    }
        public void ActiveRing(int index)
    {
        if (index < 0 || index >= ringList.Length)
            return;

        ringList[index].ActiveRing(true);
    }

    private void CheckRingsAngle()
    {
        //假設每個環正確角度都是0度，比對跟正確角度的差異，在誤差範圍內就視為正確
        bool isGameOver = true;
        for (int i = 0; i < ringList.Length; ++i)
        {
            if (ringList[i].gameObject.activeSelf)
            {
                float ringAngle = ringList[i].Angle;
                float offset = Mathf.Abs(0 - ringAngle);
                if (offset > 180)
                    offset = 360 - offset;
                if (offset > 20)
                {
                    isGameOver = false; //超過+-5度就沒有擺放正確
                    print("false");
                }
            }
            else
            {
                isGameOver = false;
            }
        }

        if (isGameOver)
        {
            Debug.Log("Set");
            Play();
        }
    }
    private void Update()
    {
        //print(ringList[0].Angle);
    }
}

