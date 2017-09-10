$(document).ready(function () {
    var idSudent;
    $('.deleteStudent').on('click',
        function () {
            var tttt = this;
            var courseId = $('#courseId').val();
            idSudent = this.dataset.id;
            $.ajax({
                url: '/Courses/RemoveStudentFromCourse',
                async: true,
                data: { idSudent: idSudent, courseId: courseId },
                cache: false,
                type: "POST",
                success: function (data) {
                    console.log("/Courses/RemoveStudentFromCourse- sucses");
                },
                error: function (xhr) {
                   
                },
                complete: function (data) {
                    $(tttt).closest('tr').remove();
                }
            });
        });

});
