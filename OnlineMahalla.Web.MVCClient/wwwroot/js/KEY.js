var loadkeys = function (keyselectname, setSignedData, sendid, dataforsign) {
    console.log(dataforsign);
    var itm = document.getElementById(keyselectname).value;

    if (itm) {
        var vo = JSON.parse(itm);
        console.log('imzolash');
        if (vo.type === "certkey") {
            CAPIWS.callFunction({ plugin: "certkey", name: "load_key", arguments: [vo.disk, vo.path, vo.name, vo.serialNumber] }, function (event, data) {
                if (data.success) {
                    var id = data.keyId;
                    postLoadKey(id, vo,false,setSignedData,sendid,dataforsign);
                } else {
                    alert(data.reason);
                }
            }, function (e) {
                alert(e);
            });
        } else if (vo.type === "pfx") {

            var id = window.sessionStorage.getItem(vo.serialNumber);
            if (id) {
                postLoadKey(id, vo, function () {
                    loadPfxKey(vo);
                }, setSignedData, sendid, dataforsign);
            } else {
                loadPfxKey(vo);
            }

        }
    }


};
var postLoadKey = function (id, vo, reload, setSignedData, sendid, dataforsign) {
    CAPIWS.callFunction({ plugin: "pkcs7", name: "create_pkcs7", arguments: [Base64.encode(dataforsign), id, 'no'] }, function (event, data) {
        if (data.success) {
            var signeddata = data.pkcs7_64;
            setSignedData(sendid, dataforsign, signeddata);
        }
        else {
            alert(data.reason);
        }
    }, function (e) {
        alert(e);
    });
};

window.document.body.onload = function (e) {
    CAPIWS.apikey([
        'main.uzasbo.uz', 'A5BA4E02DDFE300EF667CA89B1EECDC17751D730B96C38B1A222E74BAB33D9CFB8A10A06F4ED607CFC68C695611B61CD36CF812D5B24C2083C1F790F574773D9',
        'zp.uzasbo.uz', 'AC26661C8ADFF6AC117D87EE5DFCBCF3AA7B753CE0845B582B10BF26E6A062CC13F0C9A963C245E0B86FE679943837B14FF3C7E52D01B9C04AA40F654DB8280A',
        'uzasbo.mdm.uz', '9B0AAFD7298C3A5EBA5732DC13D6B936479DA8A2C08963438FBD6D947DA2BD87B2613C442AA3FF7ABB7E9A7824937F8192F73F7403DFC60AD584A0D3656813EA',
        'zp.mdm.uz', 'C445C48BD748B9F69D199D1FCEB47633DAA6956B9209E51F7E37468425B031BC672DFBD74FDB88182BDEA1A0F429F34F295BCD974F02F7B137068FE37A915864',
        'new.mdm.uz', 'B1E698CBC6B2EF94B9B65E666C8C675C4A2176F6B2261E4ED9B00AC96812317BB066A8131947FE0C74294A23B71C524C616A9EF27EF0BFD9F4081A31A32461C1',
        'oauth.mdm.uz', 'D1CA149D2BC7ED12F4DFA308FBAB274FBDA3F131BE4E690CE6AF3EB60425D6FD878BD2276A224E6B07745D5E87BD1CF38C6D3D7AA3D53342883A6756F1B52557',
        'salary.mdm.uz', '782CF4C3227EC99A33BD272F43D6F1958A56387F918AA17FFFB2ABDE71458FC2934213ADE57BB132DC454EED8E8F1C51B205839BA62DE1F7247104F7895149DA',
        'localhost', '96D0C1491615C82B9A54D9989779DF825B690748224C2B04F500F370D51827CE2644D8D4A82C18184D73AB8530BB8ED537269603F61DB0D03D2104ABF789970B',
        '127.0.0.1', 'A7BCFA5D490B351BE0754130DF03A068F855DB4333D43921125B9CF2670EF6A40370C646B90401955E1F7BC9CDBF59CE0B2C5467D820BE189C845D0B79CFC96F',
        'null', 'E0A205EC4E7B78BBB56AFF83A733A1BB9FD39D562E67978CC5E7D73B0951DB1954595A20672A63332535E13CC6EC1E1FC8857BB09E0855D7E76E411B6FA16E9D',
    ], function (event, data) {
        if (data.success) {
           // loadKeys();
        } else {
            alert(data.reason);
        }
    }, function (e) {
        alert(e);
    })
};

String.prototype.splitKeep = function (splitter, ahead) {
    var self = this;
    var result = [];
    if (splitter != '') {
        // Substitution of matched string
        function getSubst(value) {
            var substChar = value[0] == '0' ? '1' : '0';
            var subst = '';
            for (var i = 0; i < value.length; i++) {
                subst += substChar;
            }
            return subst;
        };
        var matches = [];
        // Getting mached value and its index
        var replaceName = splitter instanceof RegExp ? "replace" : "replaceAll";
        var r = self[replaceName](splitter, function (m, i, e) {
            matches.push({ value: m, index: i });
            return getSubst(m);
        });
        // Finds split substrings
        var lastIndex = 0;
        for (var i = 0; i < matches.length; i++) {
            var m = matches[i];
            var nextIndex = ahead == true ? m.index : m.index + m.value.length;
            if (nextIndex != lastIndex) {
                var part = self.substring(lastIndex, nextIndex);
                result.push(part);
                lastIndex = nextIndex;
            }
        };
        if (lastIndex < self.length) {
            var part = self.substring(lastIndex, self.length);
            result.push(part);
        };
    }
    else {
        result.add(self);
    };
    return result;
};

var getX500Val = function (s, f) {
    var res = s.splitKeep(/,[A-Z]+=/g, true);
    for (var i in res) {
        var n = res[i].search(f + "=");
        if (n !== -1) {
            return res[i].slice(n + f.length + 1);
        }
    }
    return "";
};

var listCertKeyCertificates = function (items, allDisks, diskIndex, callback) {
    if (parseInt(diskIndex) + 1 > allDisks.length) {
        callback();
        return;
    }
    CAPIWS.callFunction({ plugin: "certkey", name: "list_certificates", arguments: [allDisks[diskIndex]] }, function (event, data) {
        if (data.success) {
            for (var rec in data.certificates) {
                var el = data.certificates[rec];
                var x500name_ex = el.subjectName.toUpperCase();
                x500name_ex = x500name_ex.replace("1.2.860.3.16.1.1=", "INN=");
                x500name_ex = x500name_ex.replace("1.2.860.3.16.1.2=", "PINFL=");
                var vo = {
                    disk: el.disk,
                    path: el.path,
                    name: el.name,
                    serialNumber: el.serialNumber,
                    subjectName: el.subjectName,
                    validFrom: new Date(el.validFrom),
                    validTo: new Date(el.validTo),
                    issuerName: el.issuerName,
                    publicKeyAlgName: el.publicKeyAlgName,
                    CN: getX500Val(x500name_ex, "CN"),
                    TIN: (getX500Val(x500name_ex, "INITIALS") ? getX500Val(x500name_ex, "INITIALS") : (getX500Val(x500name_ex, "INN") ? getX500Val(x500name_ex, "INN") : getX500Val(x500name_ex, "UID"))),
                    UID: getX500Val(x500name_ex, "UID"),
                    O: getX500Val(x500name_ex, "O"),
                    T: getX500Val(x500name_ex, "T"),
                    type: 'certkey'
                };
                items.push(vo);
            }
            listCertKeyCertificates(items, allDisks, parseInt(diskIndex) + 1, callback);
        } else {
            alert(data.reason);
        }
    }, function (e) {
        alert(e);
    });
}

var fillCertKeys = function (items, callback) {
    var allDisks = [];
    CAPIWS.callFunction({ plugin: "certkey", name: "list_disks" }, function (event, data) {
        if (data.success) {
            for (var rec in data.disks) {
                allDisks.push(data.disks[rec]);
                if (parseInt(rec) + 1 >= data.disks.length) {
                    listCertKeyCertificates(items, allDisks, 0, function () {
                        callback();
                    });
                }
            }
        } else {
            alert(data.reason);
        }
    }, function (e) {
        alert(e);
    });
};

var listPfxCertificates = function (items, allDisks, diskIndex, callback) {
    if (parseInt(diskIndex) + 1 > allDisks.length) {
        callback();
        return;
    }
    CAPIWS.callFunction({ plugin: "pfx", name: "list_certificates", arguments: [allDisks[diskIndex]] }, function (event, data) {
        if (data.success) {
            for (var rec in data.certificates) {
                var el = data.certificates[rec];
                var x500name_ex = el.alias.toUpperCase();
                x500name_ex = x500name_ex.replace("1.2.860.3.16.1.1=", "INN=");
                x500name_ex = x500name_ex.replace("1.2.860.3.16.1.2=", "PINFL=");
                var vo = {
                    disk: el.disk,
                    path: el.path,
                    name: el.name,
                    alias: el.alias,
                    serialNumber: getX500Val(x500name_ex, "SERIALNUMBER"),
                    validFrom: new Date(getX500Val(x500name_ex, "VALIDFROM").replace(/\./g, "-")),
                    validTo: new Date(getX500Val(x500name_ex, "VALIDTO").replace(/\./g, "-")).toLocaleDateString(),
                    CN: getX500Val(x500name_ex, "CN"),
                    TIN: (getX500Val(x500name_ex, "INN") ? getX500Val(x500name_ex, "INN") : getX500Val(x500name_ex, "UID")),
                    UID: getX500Val(x500name_ex, "UID"),
                    O: getX500Val(x500name_ex, "O"),
                    T: getX500Val(x500name_ex, "T"),
                    type: 'pfx'
                };
                items.push(vo);
            }
            listPfxCertificates(items, allDisks, parseInt(diskIndex) + 1, callback);
        } else {
            alert(data.reason);
        }
    }, function (e) {
        alert(e);
    });
};

var fillPfxs = function (items, callback) {
    var allDisks = [];
    CAPIWS.callFunction({ plugin: "pfx", name: "list_disks" }, function (event, data) {
        if (data.success) {
            var disks = data.disks;
            for (var rec in disks) {
                allDisks.push(data.disks[rec]);
                if (parseInt(rec) + 1 >= data.disks.length) {
                    listPfxCertificates(items, allDisks, 0, function () {
                        callback();
                    });
                }
            }
        } else {
            alert(data.reason);
        }
    }, function (e) {
        alert(e);
    });
};

var loadKeysToSelect = function (keyselectname) {
    var combo = document.getElementById(keyselectname);
    combo.length = 0;
    
    var items = [];
    fillCertKeys(items, function () {

        fillPfxs(items, function () {

            for (var itm in items) {
                var vo = items[itm]
                var opt = document.createElement('option');
                opt.innerHTML = vo.TIN + " - " + vo.O + " - " + vo.CN + " - Срок до:" + vo.validTo;
                opt.value = JSON.stringify(vo);
                combo.appendChild(opt);
            }
        });

    });
};

var loadPfxKey = function (vo) {

    CAPIWS.callFunction({ plugin: "pfx", name: "load_key", arguments: [vo.disk, vo.path, vo.name, vo.alias] }, function (event, data) {
        console.log(data);
        if (data.success) {
            var id = data.keyId;

            
            window.sessionStorage.setItem(vo.serialNumber, id);
            postLoadKey(id, vo);
        } else {
            alert(data.reason);
        }
    }, function (e) {
        alert(e);
    });
};