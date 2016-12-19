# Infos about Project AsyncAndAwait

* Async and await in some kind unblocks the UI-thread
* it doesn't create a new thread
* the asynchrosity is achieved by using the MsgQ and resuming a method where it returned cos of await
* not awaiting an asynchronous method means your code runs on without waiting for method(result)
  * you may ().Wait() for that, but caution on UI-thread this get a proper deadlock
  * that can be prevented by changing the context <ConfigureAwait(false)> (in console there is no default context, so theadpool is used to continue after awaiting). Caution: must be done on the correct await - statement    
