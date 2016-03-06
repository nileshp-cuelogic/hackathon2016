/// <reference path="../jquery-1.10.2.min.js" />
$(document).ready(function () {
    $("#applicationList").change(function () {
        var applicationId = $(this).val();

        var select = $("#moduleList");
        if (!applicationId) {
            select.empty();
            select.append($('<option/>', {
                value: '',
                text: "Select Module"
            }));
            return;
        }
        $.getJSON("Dashboard/LoadApplicationModules", { applicationId: applicationId },
               function (data) {
                   select.empty();
                   select.append($('<option/>', {
                       value: '',
                       text: "Select Module"
                   }));
                   $.each(data, function (index, itemData) {
                       select.append($('<option/>', {
                           value: itemData.Value,
                           text: itemData.Text
                       }));
                   });
               });
    });

    $("#btnSubmit").click(function () {
        GetChartData();
    });

    $('.tip').tooltip();
    GetChartData();
    function GetChartData() {
        var applicationId = $("#applicationList").val();
        var moduleName = $("#moduleList").val();
        if (!applicationId) {
            applicationId = 0;
        }
        if (!moduleName)
            moduleName = null;
        var datep1 = $("#dp1").val();
        var datep2 = $("#dp2").val();

        $.getJSON("Dashboard/LoadErrorSummary", { applicationId: applicationId, moduleName: moduleName, fromDate: datep1, toDate: datep2 },
               function (data) {
                   ShowSummaryChart(data);
               });
    }


    //++++++++++++++++++++++++++++++++++ Widgets ++++++++++++++++++++++++++++++++++
    function ShowSummaryChart(ResponseData) {
        var chartData = {}
        var drilldownData = [];
        var chartData1 = [];
        $('.total-Errors')[0].childNodes[1].innerHTML = 0;
        if (ResponseData) {
            var dp1 = $("#dp1").val();
            var dp2 = $("#dp2").val();
            $('.total-Errors')[0].childNodes[1].innerHTML = ResponseData.ErrorCount;

            var AppErrors = ResponseData.ApplicationErrors;
            var AppModErrors = ResponseData.ApplicationModuleErrors;
            var color = ['#F7A35C', '#2b908f', '#90ED7D', '#5b97d5', '#ed7d31'];
            var ApplicationIds = $.unique(AppErrors.map(function (d) { return d.ApplicationId; }));
            var dData = [];
            for (var j = 0; j < AppErrors.length; j++) {
                dData.push({
                    name: AppErrors[j].ApplicatioName,
                    y: AppErrors[j].ErrorCount,
                    drilldown: AppErrors[j].ApplicationId,
                    color: color[j],
                });
            }
            chartData = [{
                name: "Error Summary", 
                data: dData
            }];

            for (var i in AppErrors) {
                var ApplicationId = AppErrors[i].ApplicationId
                var drilldownData1 = [];
                for (var item in AppModErrors) {

                    if (ApplicationId == AppModErrors[item].ApplicationId) {
                        drilldownData1.push({
                            name: AppModErrors[item].ModuleName,
                            y: AppModErrors[item].ErrorCount,
                            id: AppModErrors[item].ApplicationId,
                            dp1: dp1,
                            dp2: dp2,
                            events: {
                                click: function (e) {
                                    highChart1Modal(e.point.id, e.point.name, e.point.dp1, e.point.dp2);
                                }
                            }
                        });
                    }

                }
                drilldownData.push({
                    id: ApplicationId,
                    data: drilldownData1
                });
            }
        }




        $(function () {

            Highcharts.setOptions({
                lang: {
                    drillUpText: 'Back'
                }
            });
            $('#highChart1').highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                xAxis: {
                    type: 'category',
                    labels: {
                        rotation: 0,
                        style: {
                            fontSize: '10px'
                        }
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Number of Errors'
                    }
                },
                legend: {
                    enabled: false
                },
                tooltip: {
                    pointFormat: 'Total Errors. <b>{point.y}</b>'
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            rotation: -90,
                            color: '#000',
                            style: {
                                fontSize: '11px'
                            }
                        },
                        stacking: 'normal'
                    }
                },
                series: chartData
                ,
                drilldown: {
                    activeDataLabelStyle: {
                        color: 'black',
                        textShadow: '0 0 2px white, 0 0 2px white',
                        textDecoration: "none"
                    },

                    drillUpButton: {
                        position: {
                            y: 10,
                            x: -10
                        },
                        theme: {
                            fill: '#81c956',
                            'stroke-width': 1,
                            stroke: '#6fb147',
                            r: 6,
                            height: 16,
                            width: 30,
                            paddingLeft: 6,
                            states: {
                                hover: {
                                    fill: '#6fb147',
                                    style: {
                                        color: "#fff"
                                    }
                                },
                                select: {
                                    stroke: '#81c956',
                                    fill: '#6fb147',
                                    style: {
                                        color: "#fff"
                                    }
                                }
                            }
                        }
                    },
                    series: drilldownData


                }
            });
        });
    }

    
});
function highChart1Modal(applicationId, moduleName, fromDate, toDate) {
    
    $.getJSON("Dashboard/LoadModuleErrors", { applicationId: applicationId, moduleName: moduleName, fromDate: fromDate, toDate: toDate },
               function (data) {
                   var table = $('.modal-body')[0].childNodes[1].childNodes[3];
                   jQuery("#tblErrors tbody").empty();
                   $.each(data, function (index, itemData) {
                       var trRow = "";
                       trRow += "<tr>" + "<td> <a  target=\"_blank\" href='" + itemData.Url + "'>" + itemData.Url + "</a></td>"
                       + "<td>" + itemData.ModuleName + "</td>"
                       + "<td>" + itemData.FileName + "</td>"
                       + "<td>" + itemData.MethodName + "</td>"
                       + "<td>" + itemData.ErrorMessage + "</td>"
                       + "<td>" + itemData.StackTrace + "</td>"
                       + "<td>" + itemData.LogDate + "</td></tr>";
                       jQuery("#tblErrors tbody").append(trRow);
                   });


               });


    $('.highChart1Modal').modal('show');

}
$(function () {
    $('.datepicker').datepicker({
        dateFormat: 'mm/dd/yyyy'
    });

});



