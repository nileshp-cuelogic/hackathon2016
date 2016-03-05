$(document).ready(function() {
    $('.tip').tooltip();

    //++++++++++++++++++++++++++++++++++ Widgets ++++++++++++++++++++++++++++++++++

    // Student Summery1
    $(function() {
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
                    text: 'Number of Students'
                }
            },
            legend: {
                enabled: false
            },
            tooltip: {
                pointFormat: 'Total Students. <b>{point.y}</b>'
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
            series: [{
                name: 'Student Summary',
                data: [{
                    name: 'Grade Level',
                    y: 10457,
                    color: "#f45b5b",
                    drilldown: 'Grade Level'
                }, {
                    name: 'Gender',
                    y: 10457,
                    color: "#7cb5ec",
                    events: {
                        click: highChart1Modal
                    }
                }, {
                    name: 'Special Populations',
                    y: 10457,
                    color: "#2b908f",
                     events: {
                        click: highChart1Modal
                    }
                }],

            }],
            drilldown: {
                activeDataLabelStyle: {
                    color: 'black',
                    textShadow: '0 0 2px white, 0 0 2px white',
                    textDecoration: "none"
                },

                drillUpButton: {
                    position: {
                        y: -20,
                        x: 10
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
                series: [{
                    id: 'Grade Level',
                    name: 'Grade Level',
                    data: [{
                        name: 'Special Populations',
                        y: 10457,
                        color: "#2b908f",
                        drilldown: 'Special Populations',
                        events: {
                            click: highChart1Modal
                        }
                    }, {
                        name: 'Special Populations',
                        y: 10457,
                        color: "#2b908f",
                        events: {
                            click: highChart1Modal
                        }
                    }, {
                        name: 'Special Populations',
                        y: 10457,
                        color: "#2b908f",
                    }],
                }]

            }
        });
    });
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      // Student Summery2
    $(function() {
        Highcharts.setOptions({
            lang: {
                drillUpText: 'Back'
            }
        });
        $('#highChart2').highcharts({
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
                    text: 'Number of Students'
                }
            },
            legend: {
                enabled: false
            },
            tooltip: {
                pointFormat: 'Total Students. <b>{point.y}</b>'
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
            series: [{
                name: 'Student Summary',
                data: [{
                    name: 'Grade Level',
                    y: 10457,
                    color: "#f45b5b",
                }, {
                    name: 'Gender',
                    y: 10457,
                    color: "#7cb5ec",
                    events: {
                        click: highChart1Modal
                    }
                }, {
                    name: 'Special Populations',
                    y: 10457,
                    color: "#2b908f",
                }],

            }],
            drilldown: {
                activeDataLabelStyle: {
                    color: 'black',
                    textShadow: '0 0 2px white, 0 0 2px white',
                    textDecoration: "none"
                },

                drillUpButton: {
                    position: {
                        y: -20,
                        x: 10
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
                series: [{
                    id: 'Grade Level',
                    name: 'Grade Level',
                    data: [{
                        name: 'Special Populations',
                        y: 10457,
                        color: "#2b908f",
                        drilldown: 'Special Populations',
                        events: {
                            click: highChart1Modal
                        }
                    }, {
                        name: 'Special Populations',
                        y: 10457,
                        color: "#2b908f",
                    }, {
                        name: 'Special Populations',
                        y: 10457,
                        color: "#2b908f",
                    }],
                }]
            }
        });
    });

      // Student Summery2
    



    //++++++++++++++++++++++++++++++++++ Widgets ++++++++++++++++++++++++++++++++++

});

function highChart1Modal() {
    $('.highChart1Modal').modal('show');
}
function highChart1Modal() {
    $('.highChart1Modal').modal('show');
}
function highChart2Modal() {
    $('.highChart2Modal').modal('show');
}