$(document).ready(function() {
        var id = $('#Id').val();
        var urlStudentsCourses = location.origin + "/Courses/StudentForCourse/" + id;
        $('#studentsTable').load(urlStudentsCourses,
            function() {
                $.getScript('/js/Course/studentForCourse.js').done(function(script, textStatus) {
                        console.log(textStatus);
                    })
                    .fail(function(jqxhr, settings, exception) {
                        console.log('Can not load script/js/Courses/studentForCourse.js")');
                    });
            });

        var urlTeacherCourses = location.origin + "/Courses/TeacherForCourse/" + id;
        $('#teacherTable').load(urlTeacherCourses,
            function () {
                $.getScript('/js/Course/teachersForCourse.js').done(function(script, textStatus) {
                        console.log(textStatus);
                    })
                    .fail(function(jqxhr, settings, exception) {
                        console.log('Can not load script /js/Course/teachersForCourse.js")');
                    });
            });
    }
);