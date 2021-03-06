# Infos about Project AsyncAndAwait

* Async and await in some kind unblocks the UI-thread
* Async and await are contagious, you must use it consistently
* it doesn't create a new thread
* the asynchrosity is achieved by using the MsgQ and resuming a method where it returned because of await
* not awaiting an asynchronous method means your code runs on without waiting for method(result)
  * that's not a good idea...  
  * you may ().Wait() for that, but caution on UI-thread this get a proper deadlock
  * that can be prevented by changing the context <ConfigureAwait(false)> (in console there is no default context, so theadpool is used to continue after awaiting). Caution: must be done on the correct await - statement
* when working with CancellationToken
  * and just with async and await without a Task, then ThrowIf... throws OperationCanceledException
  * TaskCancelledException is not thrown <but is the BaseClass of OperationCanceled...>
  * that is only thrown, when doing with e.g. Task.Run(xx, ct) and Task is cancelled before even startet
  * just catch(OperationCanceledException)
  * if you just leave the method with some default-return-value and not throwing, the state of task etc. is RanToCompletion)
* For Cancelling always throw ct.ThrowIf *after* cleaning-up  
  * and catch(OperationCanceledException) and rethrow in inner methods   
* awaiting an ConfigureAwait(continueOnCaptureContext: false) behaves different depending on SynchronizaitonContext
  * startet on UI-Context (just one thread) code after await is executed by threadpool-thread
  * startet already on threadpool, await does not "return", but staying in thread and waiting. 
