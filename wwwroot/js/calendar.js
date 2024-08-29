document.addEventListener('DOMContentLoaded', function() {
    const calendar = document.getElementById('calendar');
    const today = new Date();
    
    function generateCalendar(month, year) {
        const firstDay = new Date(year, month).getDay();
        const daysInMonth = new Date(year, month + 1, 0).getDate();

        const monthNames = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
        const dayNames = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
        
        let table = '<header>' + monthNames[month] + ' ' + year + '</header>';
        table += '<table>';
        table += '<tr>';
        for (let i = 0; i < 7; i++) {
            table += '<th>' + dayNames[i] + '</th>';
        }
        table += '</tr><tr>';

        let day = 1;
        for (let i = 0; i < 35; i++) {
            if (i < firstDay || day > daysInMonth) {
                table += '<td></td>';
            } else {
                if (day === today.getDate() && month === today.getMonth() && year === today.getFullYear()) {
                    table += '<td class="today">' + day + '</td>';
                } else {
                    table += '<td>' + day + '</td>';
                }
                day++;
            }
            if (i % 7 === 6) {
                table += '</tr><tr>';
            }
        }
        table += '</tr></table>';
        
        calendar.innerHTML = table;
    }

    generateCalendar(today.getMonth(), today.getFullYear());
});
