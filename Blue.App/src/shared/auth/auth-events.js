var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var OAuthEvent = (function () {
    function OAuthEvent(type) {
        this.type = type;
    }
    return OAuthEvent;
}());
export { OAuthEvent };
var OAuthSuccessEvent = (function (_super) {
    __extends(OAuthSuccessEvent, _super);
    function OAuthSuccessEvent(type, info) {
        if (info === void 0) { info = null; }
        var _this = _super.call(this, type) || this;
        _this.info = info;
        return _this;
    }
    return OAuthSuccessEvent;
}(OAuthEvent));
export { OAuthSuccessEvent };
var OAuthInfoEvent = (function (_super) {
    __extends(OAuthInfoEvent, _super);
    function OAuthInfoEvent(type, info) {
        if (info === void 0) { info = null; }
        var _this = _super.call(this, type) || this;
        _this.info = info;
        return _this;
    }
    return OAuthInfoEvent;
}(OAuthEvent));
export { OAuthInfoEvent };
var OAuthErrorEvent = (function (_super) {
    __extends(OAuthErrorEvent, _super);
    function OAuthErrorEvent(type, reason, params) {
        if (params === void 0) { params = null; }
        var _this = _super.call(this, type) || this;
        _this.reason = reason;
        _this.params = params;
        return _this;
    }
    return OAuthErrorEvent;
}(OAuthEvent));
export { OAuthErrorEvent };
//# sourceMappingURL=auth-events.js.map