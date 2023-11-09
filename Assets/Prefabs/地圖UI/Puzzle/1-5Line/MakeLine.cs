using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Globalization;
using System;
namespace Naninovel
{
    public class MakeLine : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private LineRenderer Line1, Line2;
        public GameObject TheLine1, TheLine2;
        private Transform[] point;
        Vector3 screenpos;
        Vector3 worldpos;
        private Vector2 Drop;
        public bool Line1Done, Finished;
        bool Line1R = false;
        double Line1X1, Line1Y1, Line1X2, Line1Y2;
        double Line2X1, Line2Y1, Line2X2, Line2Y2;
        [Tooltip("The script asset to play.")]
        [ResourcePopup(ScriptsConfiguration.DefaultPathPrefix, ScriptsConfiguration.DefaultPathPrefix, emptyOption: "None (play script text)")]
        [SerializeField] private string scriptName = default;
        [TextArea(3, 10), Tooltip("The naninovel script text (commands) to execute; has no effect when `Script Name` is specified. Argument of the event (if any) can be referenced in the script text via `{arg}` expression. Conditional block commands (if, else, etc) are not supported.")]
        [SerializeField] private string scriptText = default;
        [Tooltip("Whether to automatically play the script when the game object is instantiated.")]
        [SerializeField] private bool playOnAwake = false;
        [Tooltip("Whether to disable waiting for input mode when the script is played.")]
        [SerializeField] private bool disableWaitInput = false;

        private string argument;
        public void Play()
        {
            argument = null;
            PlayScriptAsync();
        }

        void Start()
        {
            Line1 = TheLine1.GetComponent<LineRenderer>();
            Line2 = TheLine2.GetComponent<LineRenderer>();
        }
        void Update()
        {
            screenpos = Input.mousePosition;
            screenpos.z = Camera.main.nearClipPlane + 1;
            worldpos = Camera.main.ScreenToWorldPoint(screenpos);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!Line1Done)
            {
                Line1.SetPosition(0, worldpos);
                Line1X1 = worldpos.x;
                Line1Y1 = worldpos.y;
                //print(Line1X1 + "," + Line1Y1);
            }
            else if (Line1Done && !Finished)
            {
                Line2.SetPosition(0, worldpos);
                Line2X1 = worldpos.x;
                Line2Y1 = worldpos.y;
                //print(Line2X1 + "," + Line2Y1);
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (!Line1Done)
                Line1.SetPosition(1, worldpos);
            else if (Line1Done && !Finished)
                Line2.SetPosition(1, worldpos);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!Line1Done)
            {
                Line1.SetPosition(1, worldpos);
                Line1X2 = worldpos.x;
                Line1Y2 = worldpos.y;
                //print(Line1X2 + "," + Line1Y2);
                Check();
            }
            else if (Line1Done && !Finished)
            {
                Line2.SetPosition(1, worldpos);
                Line2X2 = worldpos.x;
                Line2Y2 = worldpos.y;
                //print(Line2X2 + "," + Line2Y2);
                Finish();
            }
        }
        private void Check()
        {
            if ((1.93 <= Line1X1) && (Line1X1 <= 3.75) && (2.64 <= Line1Y1) && (Line1Y1 <= 3.83))
            {
                if ((-0.19 <= Line1X2) && (Line1X2 <= 1.6) && (-3.53 <= Line1Y2) && (Line1Y2 <= -2.29))
                {
                    Line1Done = true;
                    Line1R = true;
                    print("Checked");
                }
                else
                    Line1Done = false;
            }
            if ((-0.19 <= Line1X1) && (Line1X1 <= 1.6) && (-3.53 <= Line1Y1) && (Line1Y1 <= -2.29))
            {
                if ((1.93 <= Line1X2) && (Line1X2 <= 3.75) && (2.64 <= Line1Y2) && (Line1Y2 <= 3.83))
                {
                    Line1Done = true;
                    Line1R = true;
                    print("Checked");
                }
                else
                    Line1Done = false;
            }
            if ((-1.48 <= Line1X1) && (Line1X1 <= 0.25) && (2.62 <= Line1Y1) && (Line1Y1 <= 3.75))
            {
                if ((-5.25 <= Line1X2) && (Line1X2 <= -3.87) && (-1.42 <= Line1Y2) && (Line1Y2 <= -0.11))
                {
                    Line1Done = true;
                    Line1R = false;
                    print("Checked");
                }
                else
                    Line1Done = false;
            }
            if ((-5.25 <= Line1X1) && (Line1X1 <= -3.87) && (-1.42 <= Line1Y1) && (Line1Y1 <= -0.11))
            {
                if ((-1.48 <= Line1X2) && (Line1X2 <= 0.25) && (2.62 <= Line1Y2) && (Line1Y2 <= 3.75))
                {
                    Line1Done = true;
                    Line1R = false;
                    print("Checked");
                }
                else
                    Line1Done = false;
            }
        }
        private void Finish()
        {
            if (Line1Done && Line1R)
            {
                if ((-1.48 <= Line2X1) && (Line2X1 <= 0.25) && (2.62 <= Line2Y1) && (Line2Y1 <= 3.75))
                {
                    if ((-5.25 <= Line2X2) && (Line2X2 <= -3.85) && (-1.42 <= Line2Y2) && (Line2Y2 <= -0.11))
                    {
                        print("Finish");
                        Finished = true;
                        Play();
                    }
                    //else  
                }
                if ((-5.25 <= Line2X1) && (Line2X1 <= -3.85) && (-1.42 <= Line2Y1) && (Line2Y1 <= -0.11))
                {
                    if ((-1.48 <= Line2X2) && (Line2X2 <= 0.25) && (2.62 <= Line2Y2) && (Line2Y2 <= 3.75))
                    {
                        print("Finish");
                        Finished = true;
                        Play();
                    }
                    //else
                }
            }
            if (Line1Done && !Line1R)
            {
                if ((1.93 <= Line2X1) && (Line2X1 <= 3.75) && (2.64 <= Line2Y1) && (Line2Y1 <= 3.83))
                {
                    if ((-0.19 <= Line2X2) && (Line2X2 <= 1.6) && (-3.53 <= Line2Y2) && (Line2Y2 <= -2.29))
                    {
                        print("Finish");
                        Finished = true;
                        Play();
                    }
                    //else  
                }
                if ((-0.19 <= Line2X1) && (Line2X1 <= 1.6) && (-3.53 <= Line2Y1) && (Line2Y1 <= -2.29))
                {
                    if ((1.93 <= Line2X2) && (Line2X2 <= 3.75) && (2.64 <= Line2Y2) && (Line2Y2 <= 3.83))
                    {
                        print("Finish");
                        Finished = true;
                        Play();
                    }
                    //else
                }
            }
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
    }
}