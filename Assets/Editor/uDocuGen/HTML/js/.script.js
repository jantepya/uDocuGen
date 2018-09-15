$(document).ready(function(){
    $(".core").hide();

    $(".home_button").click(function(){
        $(".core").fadeOut("slow");
        $(".home").fadeIn("slow");
    });

    $(".files").click(function(){
        $(".core").css("opacity",1);
        $($(this).attr('id').toString()).fadeIn("slow");
        $(".home").fadeOut("slow");
      });
});