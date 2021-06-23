﻿using System.Threading;
using System.Threading.Tasks;

namespace ChatdollKit.Dialog
{
    public interface ISkillRouter
    {
        void Configure();
        void RegisterIntent(string intentName, ISkill skill);
        Task ExtractIntentAsync(Request request, State state, CancellationToken token);
        ISkill Route(Request request, State state, CancellationToken token);
    }
}
