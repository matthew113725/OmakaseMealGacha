$("#spinBtn").on("click", function () {
    const selectedMeal = $('input[name="meal"]:checked').val();
    const userName = $("#userName").val().trim();

    if (!userName) {
        alert("名前を入力してください。");
        return;
    }

    $.ajax({
        url: '/Gacha/GetMenu',
        type: 'POST',
        data: {
            category: selectedMeal,
            userName: userName
        },
        dataType: 'json',
        success: function (data) {
            if (data.error) {
                alert(data.message || "エラーが発生しました。");
                return;
            }
            $("#result").text(`${data.result}`);
        },
        error: function () {
            alert("ガチャ実行中にエラーが発生しました。");
        }
    });
});