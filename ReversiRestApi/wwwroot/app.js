"use strict";

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); Object.defineProperty(Constructor, "prototype", { writable: false }); return Constructor; }

var FeedbackWidget = /*#__PURE__*/function () {
  function FeedbackWidget(elementId) {
    _classCallCheck(this, FeedbackWidget);

    this._elementId = elementId;
    this.lsName = 'feedback-widget';
  }

  _createClass(FeedbackWidget, [{
    key: "elementId",
    get: function get() {
      //getter, set keyword voor setter methode
      return this._elementId;
    }
  }, {
    key: "show",
    value: function show(message, type) {
      var element = document.getElementById(this._elementId);
      element.style.display = "block";
      element.innerText = message;

      if (type === "success") {
        element.classList.remove("alert-danger");
        element.classList.add("alert-success");
      } else {
        element.classList.remove("alert-success");
        element.classList.add("alert-danger");
      }

      this.log({
        message: message,
        type: type
      });
    }
  }, {
    key: "hide",
    value: function hide() {
      var element = document.getElementById(this._elementId);
      element.style.display = "none";
    }
  }, {
    key: "log",
    value: function log(message) {
      var arr = JSON.parse(localStorage.getItem(this.lsName));
      if (arr === null) arr = [];
      arr.push(message);
      arr.length > 10 ? arr.pop() : arr.length;
      localStorage.setItem(this.lsName, JSON.stringify(arr));
    }
  }, {
    key: "removeLog",
    value: function removeLog() {
      localStorage.removeItem(this.lsName);
    }
  }, {
    key: "history",
    value: function history() {
      var sb = '';
      var array = JSON.parse(localStorage.getItem(this.lsName));
      array.forEach(function (e) {
        return sb += "".concat(e.type, " -  ").concat(e.message, " \n");
      });
      this.show(sb);
    }
  }]);

  return FeedbackWidget;
}();

var Game = function (url) {
  console.log(url);
  var configMap = {
    apiUrl: url
  };

  var privateInit = function privateInit() {
    console.log(configMap.apiUrl);
    afterInit();

    _getCurrentGameState();
  };

  var _getCurrentGameState = function _getCurrentGameState() {
    setInterval(function () {
      Game.Model._getGameState();
    }, 2000);
  };

  return {
    init: privateInit
  };
}('https://localhost:44389/');

Game.Reversi = function () {
  console.log('hallo, vanuit module Reversi');
  var configMap;

  var init = function init() {
    Game.init();
  };

  return {
    init: init
  };
}();

Game.Model = function () {
  var configMap;
  var username;
  var data;

  var init = function init() {
    Game.init();
  };

  var _getGameState = function _getGameState() {
    var _this = this;

    Game.Data.get('api/Spel/Beurt').then(function (result) {
      switch (result[0].data) {
        case 0:
          console.log('geen specifieke waarde');
          break;

        case 1:
          console.log('wit aan zet');
          break;

        case 2:
          console.log('zwart aan zet');
          break;

        default:
          throw new Error('geen geldige waarde');
      }

      _this.data = result.data;
    });
  };

  return {
    init: init,
    _getGameState: _getGameState
  };
}();

Game.Data = function () {
  var configMap = {
    apiKey: 'c32020e3bd50f3311217e770ddc96b65',
    mock: [{
      url: 'api/Spel/Beurt',
      data: 0
    }]
  };
  var stateMap = {
    environment: 'development'
  };

  var getMockData = function getMockData(url) {
    configMap.url = url;
    var mockData = configMap.mock;
    return new Promise(function (resolve, reject) {
      resolve(mockData);
    });
  };

  var get = function get(url) {
    if (stateMap.environment === 'development') {
      return getMockData(url);
    }

    return $.get(url).then(function (r) {
      return r;
    })["catch"](function (e) {
      console.log(e.message);
    });
  };

  var init = function init(environment) {
    Game.init();
    stateMap.environment = environment;

    switch (environment) {
      case 'production':
        console.log('request server?');
        break;

      case 'development':
        getMockData(configMap.mock.url);
        break;

      default:
        throw new Error('environment fout');
    }
  };

  return {
    init: init,
    get: get
  };
}();