$(function () {
    getActiveContests("current");
    $("#tabs").tabs();
    
    $("#internaltabs").tabs();
    $("#internaltabs").on("click", "a", function () {
        var id = $(this).attr("href");
        switch (id) {
            case ("#tab-7"):
                {
                    getActiveContests("current");
                }
                break;
                // awaiting contests
            case ("#tab-8"):
                {
                    getAwaitingContests("awaiting");
                }
                break;
                // add contest tab
            case ("#tab-9"):
                {
                    unbindButtonEvents("createButton");
                    createContestButton("createButton");
                    $(document).on("submit", "#contestForm", function (e) {
                        e.preventDefault();
                        var result = doCustomValidation();
                        if (result == true) {
                            submitContestForm();
                        }
                    });
                }
                break;
            case ("#tab-10"):
                {


                }
                break;
                //competitors registration
            case ("#tab-11"):
                {


                }
                break;
            case ("#languages"):
                {
                    getLanguages("languages-container");
                }
                break;                
        }
    });    

    $("#tabs").on("click", "a", function () {
        var id = $(this).attr("href");
        switch (id) {
            case ("#tab-1"):
                {
                    getActiveContests("current");
                }
                break;
            case ("#tab-2"):
                {
                    getUsers("users-container");                    
                };
                break;
            case ("#tab-3"):
                {
                }
                break;
            case ("#tab-4"):
                {
                }
                break;
            case ("#tab-5"):
                {
                }
                break;
            case ("#tab-6"):
                {
                }
                break;
        }
    }
    );
    
    function addLanguage(containerId) {
        processLanguage(containerId, "POST", "/Administrator/AddLanguage", {});
    }

    function editLanguage(containerId, languageId) {
        processLanguage(containerId, "GET", "/Administrator/EditLanguage", { "languageId": languageId });
    }

    function deleteLanguage(containerId, languageId) {
        $.ajax({
            url: "/Administrator/DeleteLanguage",
            data: { "languageId": languageId }
        })
        .done(function (data) {
            $("#" + containerId).html(data);
        })
        .fail(function (data) {
            console.log("Error in ajax get request. Details: " + JSON.stringify(data));
        });
    }

    function processLanguage(containerId, httpMethod, actionUrl, passedData) {
        $.ajax({ 
            url:  actionUrl,
            data: passedData
        })
        .done(function (data) {
            var box = $("#" + containerId);
            box.html(data);
            $(document).on("submit", "#language-edit-form", function (e) {
                e.preventDefault();
                $.ajax({
                    url: "/Administrator/EditLanguage",
                    type: httpMethod,
                    data: {
                        'viewmodel.LanguageId': box.find("input:hidden[name='LanguageId']").val(),
                        'viewmodel.LanguageName': box.find("input[name='LanguageName']").val()
                    }
                })
                .done(function (data) {
                    box.html(data);
                })
                .fail(function (data) {
                    console.log("Error in ajax get request. Details: " + JSON.stringify(data));
                });
            });
        })
        .fail(function (data) {
            console.log("Error in ajax get request. Details: " + JSON.stringify(data));
        });
    }     

    function getLanguages(containerId) {
        $.ajax({ url: "/Administrator/ProgrammingLanguages" })
        .done(function (data) {
            $("#" + containerId).html(data);
            $("#" + containerId).on("click", "a", function () {
                var action = $(this).attr("href");
                if (action === "#add-language") {
                    addLanguage(containerId);
                } else if (action === "#edit-language") {
                    var id = $(this).attr("id");
                    editLanguage(containerId, id);
                } else if (action === "#delete-language") {
                    var id = $(this).attr("id");
                    deleteLanguage(containerId, id);
                }
            });
        })
        .fail(function (data) {
            console.log("Error in ajax get request. Details: " + JSON.stringify(data));
        });
    } 

    function getUserForEdit(userId, containerId) {
        $.ajax({
            type: "get",
            url: "/Administrator/EditUser",
            data: {
                "userId": userId
            },
            cache: false            
        }).done(function (data) {
            $("#" + containerId).html(data);

            $(document).on("submit", "#user-edit-form", function (e) {
                e.preventDefault();
                $.ajax({
                    url: "/Administrator/EditUser",
                    type: "POST",
                    data: {
                        'viewmodel.Id': box.find("input:hidden[name='Id']").val(),
                        "viewmodel.UserName": "",
                        "viewmodel.Email": "",
                        "viewmodel.NickName": box.find("input[name='NickName']").val(),
                        "viewmodel.Password": box.find("input[name='Password']").val(),
                        "viewmodel.PasswordConfirmation": box.find("input[name='PasswordConfirmation']").val()
                    }
                }).done(function (data) {
                    console.log("success!");
                }).fail(function (data) {
                    console.log("Error in ajax get request to /Administrator/EditUser. Detail: " + JSON.stringify(data));
                });
            });
        }).fail(function (data) {
            console.log("Error in ajax get request to /Administrator/EditUser. Detail: " + JSON.stringify(data));
        });
    }

    function getUsers(containerId) {
        $.ajax({ url: "/Administrator/Users" }).done(function (data) {
            $("#" + containerId).html(data);

            $("#" + containerId).on("click", "a", function () {
                var id = $(this).attr("id");

                getUserForEdit(id, containerId);
            });

        }).fail(function (data) {
            console.log("Error in ajax get request to /Administrator/Users. Detail: " + JSON.stringify(data));
        });
    }  

    function getActiveContests(containerId) {

        $.ajax({ url: "/Administrator/ActiveContests" }).done(function (data) {
            $("#" + containerId).html(data);
        }).fail(function () {
            console.log("Error in ajax get request to /Administrator/ActiveContests");
        });
    }
    
    function getAwaitingContests(containerId) {

        $.ajax({ url: "/Administrator/AwaitingContests" }).done(function (data) {
            $("#" + containerId).html(data);
        }).fail(function () { console.log("Error in ajax get request to /Administrator/AwaitingContests"); });
    }

    function createContestButton(buttonId) {
        var button = $("#" + buttonId).button({ text: true }).click(function (event) {
            event.preventDefault();
            var html = $("#addnew").html();
            if (html != "") {
                var elem = $("#dialog").dialog({
                    modal: true,
                    resizable: false,
                    height: 160,
                    buttons: {
                        "Удалить": function () {
                            $(this).dialog("close");
                            addNewContestRequest();
                        },
                        Cancel: function () {
                            $(this).dialog("close");
                        }
                    }
                });

            } else {
                addNewContestRequest();
            }
        }).css("padding-top", "7px").css("margin-top", "15px").text("Создать новый");
    }

    function unbindButtonEvents(buttonId) {
        $("#" + buttonId).unbind();
    }


    function addNewContestRequest() {
        $.ajax({
            type: "GET",
            url: "/Administrator/AddNewContest"
        }).done(function (data) {
            $("#addnew").html(data);

            var taskCount = $("#addnew").find("input[name='TasksCount']").attr("readonly", true).spinner({
                min: 1,
                max: 10
            });
            taskCount.spinner("value", 1);

            //            $("select.dropdown").selectmenu();

            $.validator.unobtrusive.parse($("#addnew"));
            $("#ContestBeginning").attr("readonly", "true").datetimepicker({
                lang: 'ru',
                formatDate: 'd.m.Y',
                startDate: new Date(),
                value: new Date(),
                step: 60,
                monthChangeSpinner: true,
                closeOnDateSelect: false,
                closeOnWithoutClick: true
            }).parent().css("margin-top", "3px");
            createAddButton(1, "tasks");
        }).fail(function () {
            console.log("Error in ajax get request to /Administrator/AddNewContest");
        });
    }

    function onSuccessCallback(elementId) {
        var counter = 0;
        var timer = setInterval(function () {
            counter++;
            if (counter == 3) {
                $("#" + elementId).text(null);
                $("#addnew").html(null);
                clearInterval(timer);
            }
        }, 1000);
    }


    // not tested
    function createAddButton(panelnumber, containerId) {
        var anchor = $("<a>").addClass("button").attr("id", "button" + panelnumber);
        var container = $("#" + containerId).append(anchor);
        anchor.button({ text: true }).click(function (event) {
            event.preventDefault();
            createTaskEditor(panelnumber);
            deleteAddButton(panelnumber);
        }).text("Добавить задачу").width(150).height(25);

        // check for error span absence and delete 
        var mas = $("#contestForm").find("span.taskscounterror");
        if (mas.lenght > 0) {
            mas.each(function () {
                $(this).remove();
            });
        }
    }


    function deleteAddButton(number) {
        var button = $("#button" + number);
        if (button) {
            button.remove();
        }
    }

    function createTaskEditor(panelNumber) {

        var panel = $("<div>").attr("id", "panel" + panelNumber).addClass("task-panel");
        var title = $("<h4>").text("Задача №" + panelNumber);

        // uncomment this for creating DELETE task button
        /* var delButton = $("<a>").attr("title", "Удалить задачу")
            .button({ text: false, icons: { primary: "ui-icon-close" } })
            .css("width", "22px")
            .css("height", "14px")
            .css("margin", "2px 5px")
            .css("position", "relative")
            .css("left", "330px")
            .click(function (event) {
            var panels = $(".task-panel");
               $.each(panels, function() {
                   var id = $(this).attr("id").match(/\d+/);
                   var k = 0;
                if (id > (panelNumber)) {
                    var newPanelNumber = panelNumber - 1;
                    var header = $(this).closest("h4.ui-accordion-header").prev();
                    header.text("Задача №" + newPanelNumber);
                }
            });
               
               var lastnumber = panels.last().attr("id").match(/\d+/);
               var button = $("#tasks").find("a.button").last();
               button.remove();
                createAddButton(lastnumber, "tasks");
                $(this).parent().remove();
                panel.remove();
            });
        title.append(delButton);*/

        $.get("/Administrator/OpenTaskEditor", function (data) {
            panel.html(data);

            var award = panel.find("input[name='TaskAward']").attr("readonly", true);
            award.spinner({
                min: 1,
                max: 100
            });

            award.spinner("value", 10);

            var duration = panel.find("input[name='TaskDuration']").attr("readonly", true);
            duration.spinner({
                min: 5,
                max: 300,
                step: 5
            });
            duration.spinner("value", 40);


            var complexity = panel.find("input[name='TaskComplexity']").attr("readonly", true);
            complexity.spinner({
                min: 1,
                max: 20
            });
            complexity.spinner("value", 5);


            panel.find("div.langlist").accordion({
                collapsible: true,
                heightStyle: "content"
            });
            $("#tasks").append(title);
            $("#tasks").append(panel);


            var accordion = $("#tasks").accordion("instance");
            if (accordion) {
                accordion.destroy();
            }
            $("#tasks").accordion({
                heightStyle: "content",
                header: "h4",
                collapsible: true
            });

            $("#tasks").accordion({ active: panelNumber - 1 });
            createAddButton(panelNumber + 1, "tasks");
        });
    }

    function deleteTaskEditor(number) {
        var panel = $("#panel" + number);
        if (panel) {
            panel.remove();
        }
    }

    function createDeleteButton(number) {
        var anchor = $("<a>").addClass("button").attr("id", "delButton" + number);
        anchor.button({ text: true }).click(function (event) {
            event.preventDefault();
            deleteTaskEditor(number);
        }).text("Удалить задачу");
    }

    function submitContestForm() {

        var tasks = {};
        var index = 0;
        var contestForm = $("#contestForm");
        contestForm.find(".task-panel").each(function () {
            var current = $(this);
            var languages = [];

            var langlist = current.find("div.langlist").first();
            var checkboxes = langlist.find("input:checked");
            var counter = 0;
            checkboxes.each(function () {
                var checkboxName = $(this).attr("name");
                var hidden = langlist.find("input:hidden[name='" + checkboxName + '.LanguageId' + "']");
                var label = langlist.find("label[name='" + checkboxName + "']");
                var val = hidden.attr("value");
                var labelText = label.text();
                languages[counter++] = {
                    LanguageId: val,
                    LanguageName: labelText
                };
            });
            
            var currentTask = {
                TaskComplexity: current.find("input[name='TaskComplexity']").val(),
                TaskDuration: current.find("input[name='TaskDuration']").val(),
                TaskAward: current.find("input[name='TaskAward']").val(),
                TaskTitle: current.find("input[name='TaskTitle']").val(),
                TaskContent: current.find("textarea[name='TaskContent']").val(),
                TaskComment: current.find("textarea[name='TaskComment']").val(),
                Languages: languages
            };
            tasks[index++] = currentTask;
        });
        
        var priorityId = $("#ContestPriorityId option:selected").attr("value");
        var dataToTransfer = {
            'viewmodel.TasksCount': contestForm.find("input[name='TasksCount']").val(),
            'viewmodel.ContestTitle': contestForm.find("input[name='ContestTitle']").val(),
            'viewmodel.ContestBeginning': contestForm.find("input[name='ContestBeginning']").val(),
            'viewmodel.TaskEditors': tasks,
            'viewmodel.ContestPriorityId': priorityId
        };

        $.ajax({
            url: "/Administrator/AddNewContest",
            type: "POST",
            data: dataToTransfer
        }).done(function (data) {
            if (data.Succeeded != null && data.Succeeded == true)
                displaySuccessfulMessage();
        }).fail(function () {
            console.log("Error in ajax POST request to /Administrator/AddNewContest");
        });
    }

    function doCustomValidation() {
        var isVal = true;
        var panels = $("#contestForm").find(".task-panel");

        //validation for tasks count
        if (panels.length == 0) {
            var length = $("#contestForm").find("span.taskscounterror").length;
            if (length == 0) {
                var span = $("<span></span>").addClass("taskscounterror")
                    .text("Соревнование должно содержать хотя бы одну задачу")
                    .css("display", "block")
                    .addClass("field-validation-error")
                    .css("margin", "10px 0;");
                $("#tasks").append(span);
            }
            return false;
        } else {
            var span = $("#contestForm").find("span.taskscounterror");
            if (span.length > 0) {
                span.each(function () {
                    $(this).remove();
                });
            }
        }
        var pans = $(document).find(".task-panel");
        $.each(pans, function () {
            var panel = $(this);
            isVal = isVal && checkEditor(panel);
        });
        return isVal;
    }

    function displaySuccessfulMessage() {
        $("#addnew").html(null);
        var counter = 0;
        var p = $("<p>").text("Создание контеста успешно завершено.");
        $("#addnew").append(p);
        var timer = setInterval(function () {
            counter++;
            if (counter == 3) {
                p.text(null);
                p.remove();
                clearInterval(timer);
            }
        }, 1000);
    }


    function checkEditor(selector) {
        var isValid = true;

        var id = selector.attr("id");

        // title validation
        var title = selector.find("input[name='TaskTitle']");
        if (!title.val()) {
            isValid = false;
            title.addClass("input-validation-error");
            var span = title.parent().find("span").first();
            span.addClass("field-validation-error").css("display", "block").css("margin-bottom", "3px");
            span.text("Необходимо указать наименование задачи");
            return isValid;
        } else {
            title.removeClass("input-validation-error");
            var span = title.parent().find("span").first();
            span.removeClass("field-validation-error");
            span.text(null);
        }

        // content validation
        var content = selector.find("textarea[name='TaskContent']");
        if (!content.val()) {
            isValid = false;
            content.addClass("input-validation-error");
            var span = content.parent().find("span").first();
            span.addClass("field-validation-error").css("display", "block").css("margin-bottom", "3px");
            span.text("Необходимо заполнить описание задачи");
            return isValid;
        } else {
            content.removeClass("input-validation-error");
            var span = content.parent().find("span").first();
            span.removeClass("field-validation-error");
            span.text(null);
        }


        // languages validation
        var langlist = selector.find("div.langlist").first();
        var cboxes = langlist.find("input:checked");
        var length = cboxes.length;
        if (length == 0) {
            isValid = false;
            var mas = langlist.find("span.checkboxeserror");
            if (mas.length == 0) {
                var span = $("<span></span>").addClass("checkboxeserror").css("display", "block").css("margin-top", "3px");
                span.addClass("field-validation-error").css("display", "inline-block").css("margin-top", "3px");
                span.text("Необходимо выбрать как минимум один ЯП");
                langlist.append(span);
            }
            return isValid;
        } else {
            var span = langlist.find("span.checkboxeserror").first();
            if (span) {
                span.remove();
            }
        }


        // comment validation
        var comment = selector.find("textarea[name='TaskComment']");
        comment.attr("data-val-range-max", 300);

        var text = comment.val();
        if (text) {
            if (text.length > 300) {
                isValid = false;
                {
                    comment.addClass("input-validation-error");
                    var span = comment.parent().find("span").first();
                    span.addClass("field-validation-error").css("display", "block").css("margin-bottom", "3px");
                    span.text("Комментарий к задаче не должен превышать длину в 300 символов");
                }

            } else {
                comment.removeClass("input-validation-error");
                var span = comment.parent().find("span").first();
                span.removeClass("field-validation-error");
                span.text(null);
            }
        }


        // others
        var complexity = selector.find("input[name='TaskComplexity']");
        if (!complexity.val()) {
            isValid = false;
            complexity.addClass("input-validation-error");
            var span = complexity.parent().find("span").first();
            span.addClass("field-validation-error").css("display", "block").css("margin-bottom", "3px");
            span.text("Необходимо указать сложность задачи");
            return isValid;
        } else {
            complexity.removeClass("input-validation-error");
            var span = complexity.parent().find("span").first();
            span.removeClass("field-validation-error");
            span.text(null);
        }


        var duration = selector.find("input[name='TaskDuration']");
        if (!duration.val()) {
            isValid = false;
            duration.addClass("input-validation-error").css("display", "block").css("margin-bottom", "3px");
            var span = duration.parent().find("span").first();
            span.addClass("field-validation-error");
            span.text("Необходимо указать время, выделяемое для задачи");
            return isValid;
        } else {
            var span = duration.parent().find("span").first();
            duration.removeClass("input-validation-error");
            span.removeClass("field-validation-error");
            span.text(null);
        }

        var award = selector.find("input[name='TaskAward']");
        var k = 0;
        if (!award.val()) {
            isValid = false;
            award.removeClass("valid");
            award.addClass("input-validation-error");
            var span = award.parent().find("span").first();
            span.removeClass("field-validation-valid").css("display", "block").css("margin-bottom", "3px");
            span.addClass("field-validation-error");
            span.text("Необходимо указать максимальное количество баллов на задачу");
            return isValid;
        } else {
            award.removeClass("input-validation-error");
            award.addClass("valid");
            var span = award.parent().find("span").first();
            span.addClass("field-validation-valid");
            span.removeClass("field-validation-error");
            span.text(null);
        }


        /* $.validator.unobtrusive.parse($("#contestForm").html());
                $("#contestForm").validate().form();*/
        return isValid;
    }

});