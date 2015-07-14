(function($) {
    $.fn.ellipsis = function(options) {

        // default option
        var defaults = {
            'row' : 1, // show rows
            'onlyFullWords': false, // set to true to avoid cutting the text in the middle of a word
            'char' : '...', // ellipsis
            'callback': function() {},
        };

        options = $.extend(defaults, options);

        this.each(function () {
            var self = this;

            // get the user-specified number of rows
            var userRows = $(self).attr('data-lines'); // TODO: change from data-lines to data-rows
            userRows && (options['row'] = parseInt(userRows));

            // make a copy of the number of lines and set as current
            var currentRows = options.row;

            // get the user-specified threshholds
            var threshholds = $(self).attr('data-threshholds');
            threshholds = threshholds ? JSON.parse(threshholds) : null;
            
            // grab a copy of the 'clean', unchanged text (ellipsis is destructive)
            var cleanText = $(self).text().trim();

            // initialize the rows based on threshholds (if any)
            var rows = getRowsFromThreshholds(threshholds, currentRows, userRows);

            // generate the ellipsis and retrieve the ACTUAL number of current rows.
            currentRows = generateEllipsis(self, rows, currentRows, cleanText);        

            // regenerate the ellipsis during page resizing
            $(window).resize(function () {
                var rows = getRowsFromThreshholds(threshholds, currentRows, userRows);
                
                currentRows = generateEllipsis(self, rows, currentRows, cleanText);
            });
        });
        
        // takes the threshholds, the 
        function getRowsFromThreshholds(threshholds, currentRows, userRows) {
            var width = window.innerWidth; // get width of page including scrollbar (+17px) if it is visible
            var rows = currentRows;

            if (threshholds) {
                for (var i = 0; i < threshholds.length; i++) {
                    var rule = threshholds[i];

                    if (!rule.hasOwnProperty('under')) {
                        var message = 'If you pass a threshholds property, you must' +
                            ' specify either an under property and a row property';
                        console.error('Error: %s', message);
                        return;
                    }
                    
                    // if we are under the threshhold, obey the row rule as specified
                    if (width < rule['under']) {
                        rows = rule.row;
                    } 
                    // we are above the threshhold, go back to the original number of user-specified rows
                    else {
                        console.log(userRows);
                        rows = userRows;
                    }
                }
            }

            return rows;
        }

        function generateEllipsis(obj, row, currentRows, cleanText) {
            //console.time('Generate Ellipsis');

            // get element text
            var $this = $(obj);
            if ($this.height() == 0) return currentRows;

            // track whether we've crossed a threshhold.
            var switchTriggered = false;

            // if we have crossed a threshhold, reset the container with
            // the clean text (the ellipsis process is destructive), and recalculate.
            var text = cleanText;
            
            // determine the length of the text.
            var origText = text;
            var origLength = origText.length;
            var origHeight = $this.height();

            // 1. get the height of a row of text
            $this.text('a');
            var lineHeight = parseFloat($this.css("lineHeight"), 10);
            var rowHeight = $this.height();
            var gapHeight = lineHeight > rowHeight ? (lineHeight - rowHeight) : 0;

            // 2. Determine height of new container based on number of rows specified
            var targetHeight = gapHeight * (row - 1) + rowHeight * row;
            
            var start = 1, length = 0;
            var end = text.length;

            // continually cut the string in half an add the ellipsis until
            // the entire string fits within the designated size.
            while (start < end) {
                // get exactly half of the length of the text
                length = Math.ceil((start + end) / 2);
                
                // cut the text string in half and append an ellipsis '...'
                $this.text(text.slice(0, length) + options['char']);

                if ($this.height() <= targetHeight) {
                    // the height is under the target height
                    start = length;
                } else {
                    // the height is still bigger than the target height...keep chopping
                    end = length - 1;
                }
            }
            
            var addEllipsis = cleanText.length != ($this.text().length - 3);
            text = text.slice(0, start);

            if (options.onlyFullWords) {
                // remove fragment of the last word together with possible soft-hyphen characters
                text = text.replace(/[\u00AD\w\uac00-\ud7af]+$/, ''); 
            }

            // append the ellipsis - '...'
            text += !textContainsEllipsis(text) && addEllipsis ? options['char'] : '';                       

            // append the text back into our container
            $this.text(text);
            $this.addClass('pk-ellipsis-rendered');

            // call the callback (empty function if not specified during init)
            options.callback.call(this);

            //console.timeEnd('Generate Ellipsis');
            return currentRows;
        }

        function textContainsEllipsis(text) {
            return text.substring(text.length - 3, text.length) == '...';
        }

        return this;
    };
}) (jQuery);
