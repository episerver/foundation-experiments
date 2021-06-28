define([
    "dojo/topic",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "epi/dependency",
    "epi/_Module",
    "epi/shell/command/_Command"
], function (
    topic,
    declare,
    lang,
    dependency,
    _Module,
    _Command) {
    return declare([_Module, _Command], {
        label: "Edit in Optimizely",
        iconClass: "epi-iconShare",

        constructor: function () {
            // summary:
            //        Constructs the object and sets up a reference to the content data store.
            // tags:
            //        public

            var registry = dependency.resolve("epi.storeregistry");
            this.store = registry.get("epi.cms.contentdata");
        },
        _execute: function () {
            this._resolveContentData(this.model[0].contentLink, lang.hitch(this, function (content) {
                var typeIdentifier = content.typeIdentifier;
                var publicUrl = "https://app.optimizely.com/v2/projects/" + content.properties.projectId;
                if (typeIdentifier === 'optimizely.developerfullstack.models.eventdata') {
                    publicUrl += '/implementation/events'
                } else if (typeIdentifier === 'optimizely.developerfullstack.models.flagdata') {
                    publicUrl += '/flags/' + content.properties.key;
                } else if (typeIdentifier === 'optimizely.developerfullstack.models.audiencedata') {
                    publicUrl += '/audiences/' + content.properties.fullStackId;
                }
                window.open(publicUrl, "_blank");
            }));
        },

        _onModelChange: function () {
            // summary:
            //        Updates canExecute after the model has been updated.
            // tags:
            //        protected
            //debugger;
            //var model = this.model,
            //    canExecute = model && !model.isCommonDraft;
            if (this.model != null) {
                var typedId = this.model[0].typeIdentifier;
                if (typedId === 'optimizely.developerfullstack.models.flagdata' ||
                    typedId === 'optimizely.developerfullstack.models.eventdata' ||
                    typedId == 'optimizely.developerfullstack.models.audiencedata') {
                    this.set("canExecute", true);
                } else {
                    this.set("canExecute", false);
                }
            } else {
                this.set("canExecute", false);
            }
        },
        _resolveContentData: function (contentlink, callback) {
            var registry = dependency.resolve("epi.storeregistry");
            var store = registry.get("epi.cms.contentdata");
            if (!contentlink) {
                return null;
            }

            var contentData;
            dojo.when(store.get(contentlink), function (returnValue) {
                contentData = returnValue;
                callback(contentData);
            });
            return contentData;
        },
    });
});