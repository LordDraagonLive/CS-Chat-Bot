using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace New_Test_Bot.Dialogs
{
    // Test message 
    // Prefarably Dialogs should be created in a different file from the controller class file

    /// <summary>
    /// This class returns the users's value back
    /// </summary>
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            // get message
            var message = await argument;
            // Return the user's message back
            await context.PostAsync("You said: " + message.Text);
            // recur the method
            context.Wait(MessageReceivedAsync);
        }

    }

    /// <summary>
    /// This class will increase count for each user message
    /// and also resets count if user typed reset in the chat
    /// </summary>
    [Serializable]
    public class EchoDialog1 : IDialog<object>
    {
        // The count diplayed to the user
        protected int count = 1;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            // get user message
            var message = await argument;

            // checks if message is set to reset by user
            if (message.Text == "reset")
            {
                // prompt a dialog for the user
                PromptDialog.Confirm(
                    context,
                    // call the AfterResetAsync func to reset the count
                    AfterResetAsync,
                    "Are you sure you want to reset the count?",
                    "Didn't get that!",
                    promptStyle: PromptStyle.None);
            }
            else
            {
                // increase count if false
                await context.PostAsync($"{this.count++}: You said {message.Text}");
                // recur this method
                context.Wait(MessageReceivedAsync);
            }

        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            // get the confirm as a bool (user value ranges between yes/y or no/n)
            var confirm = await argument;
            // if true
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(MessageReceivedAsync);
        }

    }
}