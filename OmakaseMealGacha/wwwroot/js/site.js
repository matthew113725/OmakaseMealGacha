$(document).ready(function () {

    // ガチャを回す
    $("#spinBtn").on("click", function () {
        const userName = $("#userName").val().trim();
        if (!userName) {
            alert("名前を入力してください。");
            return;
        }

        if (userName.length > 20) {
            alert("名前は20文字までです。");
            return;
        }

        $.ajax({
            url: '/Gacha/SpinGacha',
            type: 'POST',
            data: { userName },
            success: function (data) {
                if (data.error) {
                    alert(data.message);
                    return;
                }

                const resultText = $("#result");
                const menus = data.allMenus;
                let count = 0;
                const duration = 3000; // 3秒
                const interval = 100;
                const maxCount = duration / interval;

                const intervalId = setInterval(() => {
                    const randomItem = menus[Math.floor(Math.random() * menus.length)];
                    resultText.text(`どれにしようかな～　${randomItem}`);
                    count++;
                    if (count >= maxCount) {
                        clearInterval(intervalId);
                        resultText.html(`これ！→ ${data.result}`);
                        updateHistory();
                    }
                }, interval);
            },
            error: function () {
                alert("通信エラーが発生しました。");
            }
        });
    });

    // メニュー追加
    $("#addMenuForm").on("submit", function (e) {
        e.preventDefault();

        const newItem = $(this).find('input[name="newItem"]').val().trim();

        if (newItem.length > 20) {
            alert("メニュー名は20文字までです。");
            return;
        }

        $.ajax({
            url: '/Gacha/AddMenu',
            type: 'POST',
            data: { newItem },
            success: function (data) {
                if (data.error) {
                    alert(data.message);
                } else {
                    alert(data.message);
                    location.reload();
                }
            },
            error: function () {
                alert("メニュー追加中にエラーが発生しました。");
            }
        });
    });

    // メニュー編集
    $(document).on("submit", "#editMenuForm", function (e) {
        e.preventDefault();

        const $form = $(this);
        const id = $form.find('input[name="id"]').val();
        const newName = $form.find('input[name="newName"]').val();

        if (newName.length > 20) {
            alert("メニュー名は20文字までです。");
            return;
        }

        $.ajax({
            url: '/Gacha/EditMenu',
            type: 'POST',
            data: { id, newName },
            success: function (data) {
                if (data.error) {
                    alert(data.message);
                } else {
                    alert(data.message);
                    location.reload();
                }
            },
            error: function () {
                alert("メニュー編集中にエラーが発生しました。");
            }
        });
    });

    // ガチャ対象トグル処理
    $(document).on("submit", ".toggle-gacha-form", function (e) {
        e.preventDefault();

        const $form = $(this);
        const id = $form.data("id");
        const isInGacha = $form.data("in-gacha") === true || $form.data("in-gacha") === "true";

        $.ajax({
            url: '/Gacha/ToggleGacha',
            type: 'POST',
            data: {
                id: id,
                isInGacha: !isInGacha
            },
            success: function () {
                const $btn = $form.find("button");

                $btn
                    .removeClass("btn-outline-success btn-outline-secondary")
                    .addClass(!isInGacha ? "btn-outline-success" : "btn-outline-secondary")
                    .text(!isInGacha ? "ガチャ対象" : "対象外");
                    
                $form.data("in-gacha", !isInGacha);
            },
            error: function () {
                alert("状態の切り替えに失敗しました。");
            }
        });
    });
    
    // メニュー削除
    $(document).on("click", ".delete-menu-btn", function () {
        if (!confirm("本当に削除しますか？")) return;

        const $btn = $(this);
        const id = $btn.data("id");

        $.post("/Gacha/DeleteMenu", { id }, function (res) {
            if (res.success) {
                $btn.closest("li").remove();
            } else {
                alert("削除に失敗しました。");
            }
        });
    });

    // ========= ⑤ 履歴更新（共通関数） =========
    function updateHistory() {
        $.ajax({
            url: '/Gacha/GetHistory',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                const tbody = $("#history-body");
                tbody.empty();
                data.forEach(entry => {
                    const row = `
                        <tr>
                            <td>${entry.userName}</td>
                            <td>${entry.menuName}</td>
                            <td>${entry.rolledAt}</td>
                        </tr>`;
                    tbody.append(row);
                });
            },
            error: function () {
                alert("履歴の取得に失敗しました。");
            }
        });
    }
});
