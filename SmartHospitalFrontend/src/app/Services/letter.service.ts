import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Letter } from '../Interface/letter.model';

@Injectable({
  providedIn: 'root'
})
export class LetterService {
  items: Letter[] = [
    { caseNo: 1234567890, name: 'Ottovordemge, Bernd', recordingType: 'Ambulanz', doctorLetterType: 'Therapie', alterationDate: '17.03.2022', status: 'In Bearbeitung' },
    { caseNo: 1234567891, name: 'Mustermann, Eberhard', recordingType: 'Ambulanz', doctorLetterType: 'Imaging follow-up', alterationDate: '17.03.2022', status: 'In Bearbeitung' },
    { caseNo: 1234567892, name: 'Klein, Anja', recordingType: 'Ambulanz', doctorLetterType: 'Aftercare', alterationDate: '17.03.2022', status: 'In Bearbeitung' },
    { caseNo: 1234567893, name: 'John, Eberhard', recordingType: 'Ambulanz', doctorLetterType: 'Imaging follow-up', alterationDate: '17.03.2022', status: 'In Bearbeitung' },
    { caseNo: 1234567894, name: 'Kelvin, Anja', recordingType: 'Ambulanz', doctorLetterType: 'Aftercare', alterationDate: '17.03.2022', status: 'In Bearbeitung' },
    { caseNo: 1234567895, name: 'Allen, Eberhard', recordingType: 'Ambulanz', doctorLetterType: 'Imaging follow-up', alterationDate: '17.03.2022', status: 'In Bearbeitung' },
    { caseNo: 1234567896, name: 'Captain, America', recordingType: 'Ambulanz', doctorLetterType: 'Aftercare', alterationDate: '17.03.2022', status: 'In Bearbeitung' },
    { caseNo: 1234567897, name: 'Amily, Eberhard', recordingType: 'Ambulanz', doctorLetterType: 'Imaging follow-up', alterationDate: '17.03.2022', status: 'In Bearbeitung' },
    { caseNo: 1234567898, name: 'Marry, Anja', recordingType: 'Ambulanz', doctorLetterType: 'Aftercare', alterationDate: '17.03.2022', status: 'In Bearbeitung' },
    { caseNo: 1234567899, name: 'duseza, Eberhard', recordingType: 'Ambulanz', doctorLetterType: 'Imaging follow-up', alterationDate: '17.03.2022', status: 'In Bearbeitung' },
    { caseNo: 12345678910, name: 'anny, Anja', recordingType: 'Ambulanz', doctorLetterType: 'Aftercare', alterationDate: '17.03.2022', status: 'In Bearbeitung' },

  ];
 
  private apiUrl = 'https://dummy.restapiexample.com/api/v1/employees'; // Replace with your actual API endpoint
  constructor(private http: HttpClient) { }

  getData(page: number, pageSize: number): Observable<any> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get(`${this.apiUrl}`, { params });
  }

}
