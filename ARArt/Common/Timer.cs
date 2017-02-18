﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace ARArt.Common
{
    internal sealed class Timer : CancellationTokenSource
    {
        internal Timer(Action<object> callback, object state, int millisecondsDueTime, int millisecondsPeriod, bool waitForCallbackBeforeNextPeriod = false)
        {
            Task.Delay(millisecondsDueTime, Token).ContinueWith(async (t, s) => {
                var tuple = (Tuple<Action<object>, object>)s;

                while (!IsCancellationRequested) {
                    if (waitForCallbackBeforeNextPeriod) {
                        tuple.Item1(tuple.Item2);
                    } else {
                        await Task.Run(() => tuple.Item1(tuple.Item2));
                    }

                    await Task.Delay(millisecondsPeriod, Token).ConfigureAwait(false);
                }
            }, Tuple.Create(callback, state), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                Cancel();
            }

            base.Dispose(disposing);
        }
    }
}

