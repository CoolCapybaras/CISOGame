mergeInto(LibraryManager.library, {

  StartSocket: function () {
    ws = new WebSocket('ws://localhost:8887');
    ws.onopen = function() {
	  MyGameInstance.SendMessage('Canvas', 'OnMessage', '{"id":-1}');
    };

    ws.onmessage = function(e) {
	  MyGameInstance.SendMessage('Canvas', 'OnMessage', e.data);
    };

    ws.onclose = function(e) {
      console.log('Socket is closed. Reconnect will be attempted in 1 second.', e.reason);
      setTimeout(function() {
        _StartSocket();
      }, 1000);
    };

    ws.onerror = function(err) {
      console.error('Socket encountered error: ', err.message, 'Closing socket');
      ws.close();
    };
  },

  SendSocket: function (message) {
    ws.send(UTF8ToString(message));
  },

  ConsoleLog: function (message) {
    console.log(UTF8ToString(message));
  }
});
