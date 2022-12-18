function getajaxjson(apiurl, handleData) {
    $.ajax({
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json; charset=utf-8",
        },
        type: "GET",
        async: false,
        url: apiurl,
    }).done(function (data, status, xhr) {
        handleData(data);
    })
        .fail(function (xhr, status, error) {
            alert(xhr.responseText);
        });
};
function postajaxjson(apiurl, datatopost, returnurl,savemsg) {
    $.ajax({
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json; charset=utf-8"
        },
        type: "POST",
        url: apiurl,
        data: datatopost
    }).done(function (data, status, xhr) {
        alert(savemsg);
        window.location.href = returnurl;
    })
        .fail(function (xhr, status, error) {
            alert(xhr.responseText);
        });

};
function doAjaxAction(apiurl, actiontype, datatopost, handleData) {
    $.ajax({
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json; charset=utf-8"
        },
        type: actiontype,
        url: apiurl,
        data: datatopost
    }).done(function (data, status, xhr) {
        handleData(data);
    })
        .fail(function (xhr, status, error) {
            alert(xhr.responseText);
        });

};

function decimalToRussian(value) {
    return decimalToRussianF(value, 2);
}
function decimalToRussian4(value) {
    return decimalToRussianF(value, 4);
}
function decimalToRussianF(value, fraction) {
    var number = value;
    return new Intl.NumberFormat('ru-RU', { maximumFractionDigits: fraction, minimumFractionDigits: fraction }).format(number);
}
function round(value, decimals) {
    return Number(Math.round(parseFloat(value) + 'e' + decimals) + 'e-' + decimals);
}
function createselectfromurl() {

    $('select[class~="selectpicker"][data-url]').each(function (index, value) {
        var select = $(this);
        var url = $(this).attr('data-url');
        var id = $(this).attr('data-id');
        var label = $(this).attr('data-label');
        $.ajax({
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json; charset=utf-8"
            },
            type: "GET",
            async: false,
            url: url,
        }).done(function (data, status, xhr) {
            select.html('');
            select.append('<option value="0"></option>');
            $.each(data, function (key, val) {
                select.append('<option value="' + val[id] + '">' + val[label] + '</option>');
            });
            select.selectpicker('refresh');
        }).fail(function (xhr, status, error) {
                alert(xhr.responseText);
            });
    });

};
function createselectfromurlcomp(selectpicker,dataurl,addempty) {
        var select = selectpicker;
        var url = dataurl;
        var id = selectpicker.attr('data-id');
        var label = selectpicker.attr('data-label');
        $.ajax({
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json; charset=utf-8"
            },
            type: "GET",
            async: false,
            url: url,
        }).done(function (data, status, xhr) {
            select.html('');
            if (addempty) {
                select.append('<option value="0"></option>');
            }
            $.each(data, function (key, val) {
                select.append('<option value="' + val[id] + '">' + val[label] + '</option>');
            });
            select.selectpicker('refresh');
        }).fail(function (xhr, status, error) {
            alert(xhr.responseText);
        });
};
function footerStyle(row, index) {
    return {
        css: {
            "font-weight": "bold", "font-size": "12px"
        }
    };
};
function statusStyle(row, index) {
    return {
        css: {
            "font-weight": "bold", "font-size": "22px"
        }
    };
};

function totalTextFormatter(data) {
    return '����� : ';
}
function sumFormatter(data) {
    //console.log(data);
    //return 0;
    field = this.field;
    return decimalToRussian(data.reduce(function (sum, row) {        
        return sum + (+row[field]);
    }, 0));
}
function getStartDateGlobal(byDay,startDate,startMonth,startYear) {
    var startdate = startDate;
    if (byDay) {
        startdate = startDate;
    }
    else {
        if (startMonth == 1) {
            startdate = '01.' + '01.' + startYear;
        }
        else if (startMonth == 2) {
            startdate = '01.' + '02.' + startYear;
        }
        else if (startMonth == 3) {
            startdate = '01.' + '03.' + startYear;
        }
        else if (startMonth == 4) {
            startdate = '01.' + '04.' + startYear;
        }
        else if (startMonth == 5) {
            startdate = '01.' + '05.' + startYear;
        }
        else if (startMonth == 6) {
            startdate = '01.' + '06.' + startYear;
        }
        else if (startMonth == 7) {
            startdate = '01.' + '07.' + startYear;
        }
        else if (startMonth == 8) {
            startdate = '01.' + '08.' + startYear;
        }
        else if (startMonth == 9) {
            startdate = '01.' + '09.' + startYear;
        }
        else if (startMonth == 10) {
            startdate = '01.' + '10.' + startYear;
        }
        else if (startMonth == 11) {
            startdate = '01.' + '11.' + startYear;
        }
        else {
            startdate = '01.' + '12.' + startYear;

        }
    }
    return startdate;
}
function getEndDateGlobal(byDay, endDate, endMonth, endYear) {
    var enddate = endDate;
    if (byDay) {
        enddate = endDate;
    }
    else {
        var lastday = function (y, m) {
            return new Date(y, m, 0).getDate();
        }
        if (endMonth == 1) {
            enddate = lastday(endYear, 1) + '.01.' + endYear;
        }
        else if (endMonth == 2) {
            enddate = lastday(endYear, 2) + '.02.' + endYear;
        }
        else if (endMonth == 3) {
            enddate = lastday(endYear, 3) + '.03.' + endYear;
        }
        else if (endMonth == 4) {
            enddate = lastday(endYear, 4) + '.04.' + endYear;
        }
        else if (endMonth == 5) {
            enddate = lastday(endYear, 5) + '.05.' + endYear;
        }
        else if (endMonth == 6) {
            enddate = lastday(endYear, 6) + '.06.' + endYear;
        }
        else if (endMonth == 7) {
            enddate = lastday(endYear, 7) + '.07.' + endYear;
        }
        else if (endMonth == 8) {
            enddate = lastday(endYear, 8) + '.08.' + endYear;
        }
        else if (endMonth == 9) {
            enddate = lastday(endYear, 9) + '.09.' + endYear;
        }
        else if (endMonth == 10) {
            enddate = lastday(endYear, 10) + '.10.' + endYear;
        }
        else if (endMonth == 11) {
            enddate = lastday(endYear, 11) + '.11.' + endYear;
        }
        else {
            enddate = lastday(endYear, 12) + '.12.' + endYear;

        }
    }
    return enddate;
}
function filtercheckFunction() {
    // Get the checkbox
    var checkBox = document.getElementById("filtercheck");
    // Get the output text
    var text = document.getElementById("text1");
    var text = document.getElementById("text2");

    // If the checkbox is checked, display the output text
    if (checkBox.checked == true) {
        document.getElementById('text1').style.display = 'inline';
        document.getElementById('text2').style.display = 'none';
    } else {
        document.getElementById('text2').style.display = 'inline';
        document.getElementById('text1').style.display = 'none';
    }
}
function cellStyle(value, row, index) {
    if (value < 0) {
        return {
            css: {
                "color": "red",
                "font-weight": "bold",
            }
        };
    }
    return {};
}
const popupCenter = ({ url, title }) => {
    // Fixes dual-screen position                             Most browsers      Firefox
    const dualScreenLeft = window.screenLeft !== undefined ? window.screenLeft : window.screenX;
    const dualScreenTop = window.screenTop !== undefined ? window.screenTop : window.screenY;

    const width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
    const height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
    var w = width*90/100
    var h = height * 90 / 100

    const systemZoom = width / window.screen.availWidth;
    const left = (width - w) / 2 / systemZoom + dualScreenLeft
    const top = (height - h) / 2 / systemZoom + dualScreenTop
    const newWindow = window.open(url, title,
        `
      scrollbars=yes,
      width=${w / systemZoom}, 
      height=${h / systemZoom}, 
      top=${top}, 
      left=${left}
      `
    )

    if (window.focus) newWindow.focus();
}
function getParamsFromUrl(param){
    let url = document.location.href
    let params = (new URL(url)).searchParams;
    return params.get(param)
}


