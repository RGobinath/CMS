	
	//We keep flags to check whether the charts have loaded successfully.
	//By default, we assume them to false. When each chart loads, it calls
	//a JavaScript function FC_Rendered, in which we'll update the flags
	var empChartLoaded=false;
	var catChartLoaded=false;
	var prodChartLoaded=false;
	
			/**
	 * FC_Rendered function is invoked by all FusionCharts charts which are registered
	 * with JavaScript. To this function, the chart passes its own DOM Id. We can check
	 * against the DOM id and update charts or loading flags.
	 *	@param	DOMId	Dom Id of the chart that was succesfully loaded.
	*/
	function FC_Rendered(DOMId){
		//Here, we update the loaded flags for each chart
		//Since we already know the charts in the page, we use conditional loop
		switch(DOMId){
			case "TopEmployees":				
				empChartLoaded = true;
				break;
			case "SalesByCat":				
				catChartLoaded = true;
				break;
			case "SalesByProd":				
				prodChartLoaded = true;
				break;
		}
		return;
	}
	/** 
	 * updateCharts method is invoked when the user clicks on a column in the sales by year chart.
	 * In this method, we get the year as value, after which we request for XML data
	 * to update the sales by category chart and top employees chart.
	 *	@param	year	Year for which we've to show data.
	*/		
	function updateCharts(year){	
	
		//Update the checkboxes present on the same page.
		var i;
		//Iterate through all checkboxes and match the selected year
		for (i=0; i<document.forms[0].Year.length; i++){			
			if(parseInt(document.forms[0].Year[i].value,10)==year){
				document.forms[0].Year[i].checked = true;				
			}else{
				document.forms[0].Year[i].checked = false;
			}
		}		
		//Now, update the Sales by category chart, if it's loaded
		if (catChartLoaded){
			//DataURL for the categories chart
			var strURL = "Data_SalesByCategory.aspx?Year=" + year;
		
			//Sometimes, the above URL and XML data gets cached by the browser.
			//If you want your charts to get new XML data on each request,
			//you can add the following line:
			strURL = strURL + "&currTime=" + getTimeForURL();
			//getTimeForURL method is defined below and needs to be included
			//This basically adds a ever-changing parameter which bluffs
			//the browser and forces it to re-load the XML data every time.
		
			//We cache the data for the categories chart, as this data is
			//not frequently changing. So, it will enhance user's experience.
								
			//URLEncode it - NECESSARY.
			strURL = escape(strURL);		
			//Get reference to chart object using Dom ID "SalesByCat"
			
			//Send request for XML
			var chartObj = getChartFromId("SalesByCat");
			
			chartObj.setDataURL(strURL);	
		}else{
			//The chart has not loaded till now. We need to wait.
			//So either show some message to the user or do something as your requirements...
			alert("Please wait for the charts to load");
			return;
		}
		
		//Now, update the employees chart, if loaded.
		if (empChartLoaded){
			var strURL = "Data_TopEmployees.aspx?Year=" + year + "&count=5";		
			strURL = escape(strURL);
			
			var chartObj = getChartFromId("TopEmployees");
			chartObj.setDataURL(strURL);
		}else{
			alert("Please wait for the charts to load");
			return;
		}
	}
	/**
	 * updateProductChart method is called when the user selects a particular
	 * category on the category chart. Here, we send a request to get product wise
	 * XML data for the chart.
	 *	@param	intYear		Year for which the user is viewing data
	 *	@param	intMonth	Month for which drill down is required.
	 *	@param	intCatId	Category Id for which we need product wise data.
	*/
	function updateProductChart(intYear, intMonth, intCatId){	
		//Now, update the Sales By Products chart.
		
		var strURL = "Data_SalesByProd.aspx?Year=" + intYear + "&month=" + intMonth + "&catId=" + intCatId;
		strURL = escape(strURL);
		var chartObj = getChartFromId("SalesByProd");
		chartObj.setDataURL(strURL);
	}
	/**
	 * getTimeForURL method returns the current time 
	 * in a URL friendly format, so that it can be appended to
	 * dataURL for effective non-caching.
	*/
	function getTimeForURL(){
		var dt = new Date();
		var strOutput = "";
		strOutput = dt.getHours() + "_" + dt.getMinutes() + "_" + dt.getSeconds() + "_" + dt.getMilliseconds();
		return strOutput;
	}
	/**
	 * openNewWindow method helps open a new JavaScript pop-up window.
	 * It also adds the year to the end of URL
	*/	
	function openNewWindow(theURL,winName,features) {
		 window.open(theURL + "?year=" + getSelectedYear(),winName,features);
	}
	/**
	 * getSelectedYear method returns the selected year
	*/
	function getSelectedYear(){
		var selYear;
		for (i=0; i<document.forms[0].Year.length; i++){			
			if(document.forms[0].Year[i].checked){				 
				selYear = document.forms[0].Year[i].value;
			}
		}
		return selYear;
	}
		
