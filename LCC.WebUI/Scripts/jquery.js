
  	var imageCount = 1;
	 $('.left').click(function(){
		 imageCount++;
		 var image = '../../Content/Images/img'+ imageCount+'.jpg';
	 $("#slide_img").attr('src',image);	
	});


