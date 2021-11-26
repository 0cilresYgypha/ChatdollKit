﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using ChatdollKit.Dialog;
using ChatdollKit.Network;

namespace ChatdollKit.Examples.Chat
{
    public class DemoChatSkill : SkillBase
    {
        private ChatdollHttp client = new ChatdollHttp();

        private void OnDestroy()
        {
            client.Dispose();
        }

        public override async Task<Response> ProcessAsync(Request request, State state, CancellationToken token)
        {
            // Build and return response message
            var response = new Response(request.Id);

            // Get chat response message from server
            var encodedText = UnityWebRequest.EscapeURL(request.Text);
            var chatResponse = await client.GetJsonAsync<DemoChatResponse>($"https://api.uezo.net/chat/?input={encodedText}");
            if (chatResponse != null && chatResponse.outputs.Count > 0)
            {
                Random.InitState(System.DateTime.Now.Millisecond);
                response.AddVoiceTTS(chatResponse.outputs[0].val[Random.Range(0, chatResponse.outputs[0].val.Count)]);
                state.Topic.IsFinished = false; // Continue chatting
            }
            else
            {
                Debug.LogWarning("No response");
                Debug.LogWarning(chatResponse.outputs.Count);
            }

            return response;
        }

        class DemoChatResponse
        {
            public List<ResponseOutput> outputs;
        }

        class ResponseOutput
        {
            public List<float> score;
            public List<string> val;
        }
    }
}
