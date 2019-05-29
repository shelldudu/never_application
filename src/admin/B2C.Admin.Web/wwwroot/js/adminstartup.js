(function (win, undefined) {
    win.userCookie = {
        init: win.navigator.cookieEnabled ? true : false,
        setItem: function (key, value, expireTimes) {
            if (!this.init) {
                return;
            }
            var expires = new Date();
            if (expireTimes == undefined) expireTimes = 24 * 60 * 60;
            expires.setTime(expires.getTime() + expireTimes);
            document.cookie = key + "=" + escape(value) + ((expireTimes == null) ? "" : ";expires=" + expires.toGMTString() + "; path=/")
        },
        getItem: function (key) {
            if (!this.init) {
                return;
            }
            if (document.cookie.length > 0) {
                var start = document.cookie.indexOf(key + "=");
                if (start != -1) {
                    start = start + key.length + 1;
                    var end = document.cookie.indexOf(";", start);
                    if (end == -1) end = document.cookie.length;
                    return unescape(document.cookie.substring(start, end));
                }
            }
            return ""
        },
        clear: function () {
            if (!this.init) {
                return;
            }
            if (document.cookie.length > 0) {
                document.cookie.clear();
            }
        },
        removeItem: function (key) {
            if (!this.init) {
                return;
            }
            if (document.cookie.length > 0) {
                var old = UserCookie.getItem(key);
                if (old != null) UserCookie.setItem(key, old, -999);
            }
        }
    };
    win.userLocalStorage = {
        init: win.localStorage ? true : false,
        setItem: function (key, value, expireTimes) {
            if (!this.init) {
                return;
            }
            var expires = new Date();
            var record = {
                value: JSON.stringify(value),
                expires: expires.getTime() + expireTimes
            };
            win.localStorage.setItem(key, JSON.stringify(record));
        },
        getItem: function (key) {
            if (!this.init) {
                return;
            }
            var value = "";
            var record = JSON.parse(win.localStorage.getItem(key));
            if (record != null && new Date().getTime() <= record.expires) {
                value = record.value.replace(/"/g, "");
            }
            return value;
        },
        clear: function () {
            if (!this.init) {
                return;
            }
            win.localStorage.clear();
        },
        removeItem: function (key) {
            if (!this.init) {
                return;
            }
            win.localStorage.removeItem(key);
        }
    };
    win.userData = {
        userData: null,
        onlyUseCookie: false,
        name: location.hostname,
        expireTime: 24 * 60 * 60 * 1000,
        setItem: function (key, value, expirtTime) {
            if (this.onlyUseCookie) {
                if (win.userCookie.init) {
                    if (expirtTime) {
                        win.userCookie.setItem(key, value, expirtTime);
                    } else {
                        win.userCookie.setItem(key, value, UserData.expireTime);
                    }
                }
            } else {
                if (win.userLocalStorage.init) {
                    if (expirtTime) {
                        win.userLocalStorage.setItem(key, value, expirtTime);
                    } else {
                        win.userLocalStorage.setItem(key, value, UserData.expireTime);
                    }
                }
            }
        },
        getItem: function (key) {
            if (this.onlyUseCookie) {
                if (win.userCookie.init) {
                    return win.userCookie.getItem(key);
                }
            } else {
                if (win.userLocalStorage.init) {
                    return win.userLocalStorage.getItem(key);
                }
            }
        },
        removeItem: function (key) {
            if (this.onlyUseCookie) {
                if (win.userCookie.init) {
                    return win.userCookie.removeItem(key);
                }
            } else {
                if (win.userLocalStorage.init) {
                    return win.userLocalStorage.removeItem(key);
                }
            }
        }
    };
})(window);
(function (win, undefined) {
    win.reqs = {
        getPara: function (key) {
            if (key == null || key == undefined)
                return "";
            var url = window.location.search;
            var theRequest = new Object();
            if (url.indexOf("?") != -1) {
                var str = url.substr(1);
                strs = str.split("&");
                for (var i = 0; i < strs.length; i++) {
                    theRequest[strs[i].split("=")[0].toLowerCase()] = unescape(strs[i].split("=")[1]);
                }
                if (theRequest[key.toLowerCase()] == undefined)
                    return "";

                return theRequest[key.toLowerCase()];
            }

            return "";
        }
    };
})(window);
(function (win, undefined) {
    //页面加载完后执行
    win.onloadEvent = {};
    win.onloadEvent.attach = function (funcName, cdt) {
        if (cdt != "") {
            if (win.addEventListener) {
                win.addEventListener('load',
                    function () {
                        if (eval("" + cdt + "")) {
                            eval("" + funcName + "");
                        }
                    },
                    false);
            } else if (win.attachEvent) {
                win.attachEvent('onload',
                    function () {
                        if (eval("" + cdt + "")) {
                            eval("" + funcName + "");
                        }
                    },
                    false);
            }
        } else {
            if (win.addEventListener) {
                win.addEventListener('load',
                    function () {
                        eval("" + funcName + "");
                    },
                    false);
            } else if (win.attachEvent) {
                win.attachEvent('onload',
                    function () {
                        eval("" + funcName + "");
                    },
                    false);
            }
        }
    };
})(window); (function (win, undefined) {
    win.template = {};
    win.template.replaceReg = function (text) {
        var reg = {};
        if (text == "") return reg;
        var regex = new RegExp("\\{\\w+\\}", "g");
        var matchs = text.match(regex);
        if (matchs == null) return reg;
        for (var i = 0,
            j = matchs.length; i < j; i++) {
            var match = matchs[i];
            var name = match.replace("\"", "").replace("{", "").replace("}", "");
            if (reg[name] == undefined) {
                reg[name] = new RegExp(match, "g");
            }
        }
        return reg;
    };
    win.template.paging = function (pageNow, pageLine, pageCount) {
        if (pageCount <= 0) return new Array();
        if (pageNow > pageCount) pageNow = pageCount;
        var min = 1;
        if (pageNow > 2) {
            if (pageNow + pageLine >= pageCount + 2) {
                min = pageCount - pageLine + 1;
                if (min <= 1)
                    min = 1;
            } else {
                min = pageNow - 1;
            }
        }
        if (min > pageCount) min = pageCount;

        var max = pageLine + (min - 1);
        if (max > pageCount) max = pageCount;

        var array = new Array();
        for (var i = min; i <= max; i++) {
            array.push(i);
        }
        return array.reverse();
    };
})(window); (function (win, undefined) {
    if (typeof (jQuery) == undefined) return;
    var ext = jQuery;
    ext.format = function (source, params) {
        if (params == null) params = "";
        if (arguments.length == 1) return function () {
            var args = ext.makeArray(arguments);
            args.unshift(source);
            return ext.format.apply(this, args);
        };
        if (arguments.length > 2 && params.constructor != Array) {
            params = ext.makeArray(arguments).slice(1);
        }
        if (params.constructor != Array) {
            params = [params];
        }
        ext.each(params,
            function (i, n) {
                source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
            });

        return source.replace(new RegExp("\\{\\d{1,}\\}", "g"), "");
    };
    ext.formatDateTime = function (date, fmt) {
        if (typeof (date) == "string") {
            if (date.indexOf("Date") > 0) {
                date = new Date(parseInt(date.replace("/Date(", "").replace(")/", ""), 10));
            } else {
                date = new Date(date.replace("T", " ").replace("Z", "").replace("/", "-"));
            }
        }

        if (fmt == null || fmt == undefined) fmt = "yyyy-MM-dd HH:mm";

        var o = {
            //季度
            "q+": Math.floor((date.getMonth() + 3) / 3),
            //月份
            "M+": date.getMonth() + 1,
            //日
            "d+": date.getDate(),
            //小时
            "h+": date.getHours(),
            "H+": date.getHours(),
            //分
            "m+": date.getMinutes(),
            //秒
            "s+": date.getSeconds(),
            //毫秒
            "S": date.getMilliseconds()
        };

        if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));

        for (var k in o) if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));

        return fmt;
    };
    ext.formatNumber = function (nData, opts) {
        opts = ext.extend({},
            {
                decimalSeparator: ".",
                thousandsSeparator: ",",
                decimalPlaces: 0,
                round: false,
                prefix: "",
                suffix: "",
                defaulValue: 0
            },
            opts);
        if (!(typeof (nData) === 'number' && isFinite(nData))) {
            nData *= 1;
        }
        if (typeof (nData) === 'number' && isFinite(nData)) {
            var bNegative = (nData < 0);
            nData = Math.abs(nData);
            var sOutput = nData + "";
            var sDecimalSeparator = (opts.decimalSeparator) ? opts.decimalSeparator : ".";
            var nDotIndex;
            if (typeof (opts.decimalPlaces) === 'number' && isFinite(opts.decimalPlaces)) {
                // Round to the correct decimal place
                var nDecimal, nDecimalPlaces = opts.decimalPlaces;
                if (opts.round) {
                    nDecimal = Math.pow(10, nDecimalPlaces);
                    sOutput = Math.round(nData * nDecimal) / nDecimal + "";
                } else {
                    nDecimal = Math.pow(10, (nDecimalPlaces + 3));
                    sOutput = Math.floor(Math.round(nData * nDecimal) / 1000) / (nDecimal / 1000) + "";
                }
                nDotIndex = sOutput.lastIndexOf(".");
                if (nDecimalPlaces > 0) {
                    // Add the decimal separator
                    if (nDotIndex < 0) {
                        sOutput += sDecimalSeparator;
                        nDotIndex = sOutput.length - 1;
                    }
                    // Replace the "."
                    else if (sDecimalSeparator !== ".") {
                        sOutput = sOutput.replace(".", sDecimalSeparator);
                    }
                    // Add missing zeros
                    while ((sOutput.length - 1 - nDotIndex) < nDecimalPlaces) {
                        sOutput += "0";
                    }
                }
            }
            if (opts.thousandsSeparator) {
                var sThousandsSeparator = opts.thousandsSeparator;
                nDotIndex = sOutput.lastIndexOf(sDecimalSeparator);
                nDotIndex = (nDotIndex > -1) ? nDotIndex : sOutput.length;
                var sNewOutput = sOutput.substring(nDotIndex);
                var nCount = -1;
                for (var i = nDotIndex; i > 0; i--) {
                    nCount++;
                    if ((nCount % 3 === 0) && (i !== nDotIndex) && (!bNegative || (i > 1))) {
                        sNewOutput = sThousandsSeparator + sNewOutput;
                    }
                    sNewOutput = sOutput.charAt(i - 1) + sNewOutput;
                }
                sOutput = sNewOutput;
            }
            // Prepend prefix
            sOutput = (opts.prefix) ? opts.prefix + sOutput : sOutput;
            // Append suffix
            sOutput = (opts.suffix) ? sOutput + opts.suffix : sOutput;
            return (bNegative ? '-' : '') + sOutput;
        } else {
            return opts.defaulValue;
        }
    };
    ext.formatCurrency = function (num, opts, length) {
        if (typeof (length) != "number") length = 2;

        opts = ext.extend({},
            {
                decimalSeparator: ".",
                thousandsSeparator: ",",
                decimalPlaces: length,
                round: false,
                prefix: "",
                suffix: "",
                defaulValue: 0
            },
            opts);

        return $.formatNumber(num, opts);
    };
    ext.addDays = function (date, days) {
        if (date instanceof String) {
            if (date.indexOf("Date") > 0) {
                date = new Date(parseInt(date.replace("/Date(", "").replace(")/", ""), 10));
            } else {
                date = new Date(date);
            }
        }
        if (date instanceof Date) {
            date.setDate(date.getDate() + days);
        }
        return date;
    };
    ext.addMinutes = function (date, minutes) {
        if (date instanceof String) {
            if (date.indexOf("Date") > 0) {
                date = new Date(parseInt(date.replace("/Date(", "").replace(")/", ""), 10));
            } else {
                date = new Date(date);
            }
        }
        if (date instanceof Date) {
            date.setMinutes(date.getMinutes() + minutes);
        }
        return date;
    };
    ext.addSeconds = function (date, seconds) {
        if (date instanceof String) {
            if (date.indexOf("Date") > 0) {
                date = new Date(parseInt(date.replace("/Date(", "").replace(")/", ""), 10));
            } else {
                date = new Date(date);
            }
        }
        if (date instanceof Date) {
            date.setSeconds(date.getSeconds() + seconds);
        }
        return date;
    };
})(window);

(function ($) {
    $.fn.initPage = function (options) {
        var defaults = {
            pageNow: 1,
            pageSize: 15,
            totalCount: 1
        };
        var opts = $.extend(defaults, options);
        this.init = function () {
            var div = this;
            if (div == undefined) {
                return;
            }
            if (opts.pageSize == 0) {
                return;
            }
            var divHtml = "";

            var page = {
                lines: 5,/*一共显示5页来翻页*/
                now: options.pageNow,/*当前页*/
                count: Math.ceil(opts.totalCount * 1.0 / opts.pageSize),/*计算出一共有多少页*/
            };

            divHtml += $.format("<span>一共{0}条记录,共{1}页</span>", opts.totalCount, page.count);
            divHtml += "<ul class='pagination pagination-sm no-margin pull-right'>";
            if (page.count <= 0) {
                divHtml += "<li class='paginate_button previous disabled'><a href='javascript:void(0)'>← <span>上一页</span></a></li>";
                divHtml += "<li class='disabled'><a href='javascript:void(0)'><span>暂无记录</span></a></li>";
                divHtml += "<li class='next disabled'><a href='javascript:void(0)'>→ <span>下一页</span></a></li>";
            }
            else {
                var array = window.template.paging(page.now, page.lines, page.count);
                var length = array.length;
                for (var i = 1; i <= length; i++) {
                    var item = array.pop();
                    if (item == undefined)
                        continue;

                    /*要加第一页的箭头*/
                    if (i == 1) {
                        if (page.now <= 1) {
                            divHtml += "<li class='paginate_button previous disabled'><a href='javascript:void(0)'>← <span>上一页</span></a></li>";
                        }
                        else {
                            divHtml += $.format("<li class='paginate_button previous'><a href='javascript:void(0)' pageNow='{0}'>← <span class=''>上一页</span></a></li>", page.now - 1);
                        }
                    }

                    /*等于item的值不用加link*/
                    if (item == page.now) {
                        divHtml += $.format("<li class='paginate_button active'><a href='javascript:void(0)' pageNow ='{0}'>{0}</a></li>", item);
                    }
                    else {
                        divHtml += $.format("<li class='paginate_button'><a href='javascript:void(0)' pageNow ='{0}'>{0}</a></li>", item);
                    }

                    /*最后一页的箭头*/
                    if (i == length) {
                        if (page.now >= page.count) {
                            divHtml += "<li class='paginate_button disabled'><a href='javascript:void(0)'>→ <span>下一页</span></a></li>";
                        }
                        else {
                            divHtml += $.format("<li class='paginate_button'><a href='javascript:void(0)' pageNow='{0}'><span class=''>下一页</span> →</a></li>", page.now + 1);
                        }
                    }
                }
            }
            divHtml += "</ul>";
            $(div).html(divHtml);
        };
        return this.each(function () {
        });
    }
})(jQuery);

(function (window, undefined) {
    window.common = window.common || {};
    window.common.dialog = {
        title: "提示信息",
        okText: "确定",
        cancelText: "取消",
        height: 120,
        width: 300,
        //提示框(提示消息, 点击确定后的JS事件, 弹出框ID, 弹出的高度, 弹出的宽度).
        alert: function (message, callback, id, width, height) {
            id = id || "dialog" + Math.random(),
                height = height || this.height;
            width = width || this.width;
            if (frameElement == null || !frameElement.api || frameElement.api == undefined) {
                //根页面弹出
                $.dialog({
                    id: id,
                    lock: true,
                    fixed: true,
                    title: this.title || "系统提示",
                    content: message,
                    width: width,
                    height: height,
                    min: false,
                    max: false,
                    okVal: this.okText || "Ok",
                    ok: callback || true
                });
            } else {
                //弹出的页面再弹出
                frameElement.api.opener.$.dialog({
                    id: id,
                    lock: false,
                    fixed: true,
                    title: typeof (dialog) == "undefined" ? "系统提示" : dialog.title,
                    content: message,
                    width: width,
                    height: height,
                    min: false,
                    max: false,
                    okVal: typeof (dialog) == "undefined" ? "ok" : dialog.okText,
                    ok: callback || true
                });
            }
            return false;
        },
        //确认提示框(提示消息, 点击确定后的JS事件, 弹出框ID, 弹出的高度, 弹出的宽度)
        confirm: function (message, callback, id, width, height) {
            id = id || "dialog" + Math.random(),
                height = height || this.height;
            width = width || this.width;
            if (frameElement == null || !frameElement.api || frameElement.api == undefined) {
                //根页面弹出
                $.dialog({
                    id: id,
                    lock: true,
                    fixed: true,
                    title: this.title || "系统提示",
                    content: message,
                    width: width,
                    height: height,
                    min: false,
                    max: false,
                    ok: callback || true,
                    cancel: true,
                    okVal: this.okText || "Ok",
                    cancelVal: this.cancelText || "Cancel"
                });
            }
            else {
                //弹出的页面再弹出
                frameElement.api.opener.$.dialog({
                    id: id,
                    lock: true,
                    fixed: true,
                    title: this.title || "系统提示",
                    content: message,
                    width: width,
                    height: height,
                    min: false,
                    max: false,
                    ok: callback || true,
                    cancel: true,
                    okVal: this.okText || "Ok",
                    cancelVal: this.cancelText || "Cancel"
                });
            }
            return false;
        },
        //添加按钮, 以及为按钮指定点击事件(按钮上的文字, 点击按钮的JS函数)
        addButton: function (name, callback, id) {
            var api = frameElement.api;
            var win = api.opener;
            id = id || "dialog" + Math.random(),
                name = name || this.okText || "确定";
            api.button({
                id: id,
                focus: true,
                name: name,
                callback: callback
            });
        },
        // 添加关闭按钮.
        addCancelButton: function (buttonText) {
            var api = frameElement.api;
            var win = api.opener;
            var id = id || "dialog" + Math.random();
            var name = buttonText || this.cancelText || "Cancel";
            api.button({
                id: id,
                name: name,
                callback: api.cancel
            });
            return false;
        },
        // 添加按钮组(确定和关闭)
        addButtons: function (callback, okButtonText, cancelButtonText) {
            okButtonText = okButtonText || this.okText;
            cancelButtonText = cancelButtonText || this.cancelText;
            this.addButton(okButtonText, callback);
            this.addCancelButton(cancelButtonText);
        },
        //打开对话框窗口(窗口标题, 页面地址, 弹出框ID, 弹出的高度, 弹出的宽度)
        open: function (title, url, id, width, height) {
            id = id || ("dialog" + Math.random()),
                title = title || this.title;
            height = height || this.height;
            width = width || this.width;
            if (frameElement == null || !frameElement.api || frameElement.api == undefined) {
                //根页面弹出
                $.dialog({
                    id: id,
                    lock: false,
                    fixed: true,
                    title: title,
                    content: 'url:' + url,
                    width: width,
                    height: height,
                    min: false,
                    max: false
                });
            } else {
                //弹出的页面再弹出
                frameElement.api.opener.$.dialog({
                    id: id,
                    lock: false,
                    fixed: true,
                    title: title,
                    content: 'url:' + url,
                    width: width,
                    height: height,
                    min: false,
                    max: false
                });
            }
            return false;
        },
        //Loading 加载数据
        loading: function (loadingText, appPath, id) {
            var wrap = $(document.createElement("DIV"));
            wrap.attr("id", id)
                .css({
                    "background": "#808080",
                    "opacity": "0.5",
                    "border-radius": "8px",
                    "width": "120px",
                    "height": "40px",
                    "padding": "20px",
                    "margin-left": "-40px",
                    "margin-top": "-50px",
                    "position": "absolute",
                    "top": "50%",
                    "left": "50%",
                    "text-align": "center"
                })
                .appendTo($("body"));
            var imgObj = new Image();
            imgObj.src = appPath;
            var img = $(document.createElement("IMG"));
            img.attr("src", imgObj.src)
                .css({
                    "width": "28px",
                    "height": "28px"
                })
                .appendTo(wrap);

            var text = $(document.createElement("SPAN"));
            text.css({
                "width": "80px",
                "font-size": "12px",
                "font-family": "宋体",
                "color": "#ffffff",
                "display": "inline-block",
                "margin-top": "10px"
            })
                .html(loadingText)
                .appendTo(wrap);
        },
        //opeartion 操作结果
        operation: function (result) {
            if (result == null || result == undefined) {
                this.alert("操作失败");
                return;
            }
            switch (parseInt(result.status)) {
                case 0: {
                    this.alert((result.message == null || result.message == "") ? "操作失败" : result.message);
                } return;
                case -1: {
                    this.alert((result.message == null || result.message == "") ? "操作失败" : result.message);
                } return;
                case -2: {
                    var url = "";
                    if (result.result) {
                        if (result.result.PageUrl)
                            url = result.result.PageUrl;
                        else if (result.result.pageUrl)
                            url = result.result.pageUrl;
                    }
                    if (url == "") {
                        this.alert((result.message == null || result.message == "") ? "你没有权限操作" : result.message);
                    }
                    else {
                        this.confirm((result.message == null || result.message == "") ? "你没有权限操作" : result.message, function () {
                            window.location = url;
                        });
                    }
                } return;
                case -3: {
                    this.alert((result.message == null || result.message == "") ? "操作错误" : result.message);
                } return;
                case -4: {
                    this.alert((result.message == null || result.message == "") ? "操作异常，请联系技术" : result.message);
                } return;
                case 1: {
                    this.alert((result.message == null || result.message == "") ? "操作成功" : result.message);
                } return;
            }
            this.alert("操作失败");
            return;
        }
    };
})(window);

(function ($) {
    $.fn.bindEngine = function (options) {
        var defaults = {
            ajaxUrl: "",
            pageNow: 1,
            pageSize: 15,
            template: "",
            pagetemplate: "",
            dataBindFormat: {},
            onDataBinding: function () { },
            onBeforeSend: function () { },
            onSuccess: function () { },
            onDataBound: function () { },
            onRowDataBound: function () { },
            onPreDrawing: function () { }
        };
        var opts = $.extend(defaults, options);
        opts.body = this;
        opts.templateHtml = $(opts.template).text();
        this.refresh = function () {
            engine.requestData(opts, 1);
        };
        this.refreshCurrentPage = function () {
            engine.requestData(opts, opts.pageNow);
        };
        this.init = function (timeout) {
            engine.init(opts);
            setTimeout(() => {
                engine.requestData(opts, 1);
            }, timeout);

            return this;
        };
        return this.each(function () {
        });
    };
    var engine = {
        options: {},
        init: function (options) {
            this.options = options;
        },
        view: function (template, result) {
            if (template == undefined || template == null)
                return;

            template.onPreDrawing(result);
            var html = "";
            var groups = replaceReg.group(template.templateHtml); ``
            if (result.result.Records.length > 0) {
                for (var i = 0, j = result.result.Records.length; i < j; i++) {
                    var data = result.result.Records[i];
                    if (groups["className"] != undefined) {
                        html += template.templateHtml.replace(groups["className"], i % 2 == 0 ? 'odd' : 'even');
                    }
                    else {
                        html += template.templateHtml;
                    }
                    for (var p in data) {
                        if (groups[p] == undefined)
                            continue;
                        var reg = groups[p];
                        if (template.dataBindFormat[p] != undefined && template.dataBindFormat[p] instanceof Function)
                            html = html.replace(reg, template.dataBindFormat[p](data[p], data));
                        else {
                            html = html.replace(reg, data[p]);
                        }
                    }
                    template.onRowDataBound(result.result.Records[i]);
                }
            }
            $(template.body).find("table>tbody").eq(0).html(html);
            template.onDataBound(result);
        },
        requestData: function (template, pageNow) {
            if (template == undefined || template == null)
                return;
            var params = {};
            var pluginParams = template.onDataBinding();
            for (var p in pluginParams) {
                params[p] = pluginParams[p];
            }
            this.options.pageNow = pageNow;
            this.options.pageSize = template.pageSize;
            params.pageNow = pageNow;
            params.pageSize = template.pageSize;
            var that = this;
            $.ajax({
                data: params,
                type: "post",
                dataType: "json",
                url: template.ajaxUrl,
                beforeSend: function (xhr) {
                    template.onBeforeSend(xhr);
                },
                success: function (result) {
                    template.onSuccess();
                    if (result == null) {
                        window.common.dialog.alert("加载数据失败");
                        return;
                    }
                    if (result.status != 1) {
                        window.common.dialog.operation(result);
                        return;
                    }
                    that.view(template, result);
                    if ($(template.pagetemplate) != undefined) {
                        $(template.pagetemplate).initPage({
                            pageNow: pageNow,
                            pageSize: that.options.pageSize,
                            totalCount: result.result.TotalCount
                        }).init();
                        that.bindOnClick(template);
                    };

                    if (result.status == 1 && result.result.TotalCount == 0) {
                        window.common.dialog.alert("暂无数据");
                        return;
                    }
                },
                error: function (result) {
                    $("#picloading").remove();
                    window.common.dialog.alert("加载数据失败");
                }
            });
        },
        bindOnClick: function (template) {
            var ul = template.pagetemplate.find("ul.pagination").eq(0);
            if (ul == undefined)
                return;
            var that = this;
            $(ul).find("li > a").each(function () {
                var pageNow = parseInt($(this).attr("pageNow"));
                var parentClass = $(this).parent().attr("class");
                var parentClassSplit = (parentClass == undefined ? "" : parentClass).split(' ');
                for (var i = 0, j = parentClassSplit.length; i < j; i++) {
                    if (parentClassSplit[i] == "active" || parentClassSplit[i] == "disabled") {
                        return;
                    }
                }
                $(this).click(function () {
                    that.requestData(template, pageNow);
                });
            });
        }
    };
    var replaceReg = {
        group: function (text) {
            var reg = {};
            if (text == "")
                return reg;
            var regex = new RegExp("\\{\\w+\\}", "g");
            var matchs = text.match(regex);
            if (matchs == null)
                return reg;
            for (var i = 0, j = matchs.length; i < j; i++) {
                var match = matchs[i];
                var name = match.replace("\"", "").replace("{", "").replace("}", "");
                if (reg[name] == undefined) {
                    reg[name] = new RegExp(match, "g");
                }
            }
            return reg;
        }
    };
})(jQuery);

$(function () {
    $('input[bind]').each(function () {
        var value = $(this).attr("bind");
        switch (value) {
            case "date": {
                var format = $(this).attr("format");
                switch (format) {
                    case "yyyy-MM-dd HH:mm:ss": {
                        laydate.render({ elem: this, theme: 'grid', type: 'datetime' });
                    } break;
                    default: {
                        laydate.render({ elem: this, theme: 'grid' });
                    } break;
                }
            } break;
            case "checkbox_all": {
                $(this).change(function () {
                    var theader = $(this).parent().parent().parent();
                    var tbody = theader.next();
                    var checked = $(this).is(":checked");
                    $(tbody).find("tr td input[type='checkbox']").each(function () {
                        if (checked) {
                            $(this).prop("checked", true);
                        } else {
                            $(this).prop("checked", false);
                        };
                    });
                });

            } break;
        }
    });

    $('select[bind]').each(function () {
        var value = $(this).attr("bind");
        switch (value) {
            case "select2": {
                $(this).select2();
            } break;
        }
    });
    $('textarea[bind]').each(function () {
        var value = $(this).attr("bind");
        switch (value) {
            case "rich": {
                CKEDITOR.replace($(this).prop("id"));
            } break;
        }
    });

    $('.fancybox-button').each(function () {
        $(this).fancybox({
            groupAttr: 'data-rel',
            prevEffect: 'none',
            nextEffect: 'none',
            closeBtn: true,
            helpers: {
                title: {
                    type: 'inside'
                }
            }
        });
    });

    $(".nav-tabs > li > a").each(function () {
        $(this).click(function () {
            var href = $(this).attr("phref");
            if (href != null || href != undefined) {
                arguments[0].preventDefault();
                window.location.href = href;
                return;
            }
            var ohref = $(this).attr("ohref");
            if (ohref != null || ohref != undefined) {
                arguments[0].preventDefault();
                window.open(ohref, "_blank");
                return;
            }
            return true;
        });
    });
});