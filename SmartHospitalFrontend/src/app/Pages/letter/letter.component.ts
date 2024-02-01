import { Component, ViewEncapsulation } from '@angular/core';
import { LetterService } from '../../Services/letter.service';

@Component({
  selector: 'app-letter',
  templateUrl: './letter.component.html',
  styleUrls: ['./letter.component.css'],
  encapsulation: ViewEncapsulation.None,

})
export class LetterComponent {
  constructor(private dataService: LetterService) { }
  keys = Object.keys(this.dataService.items[0]);
  columnMappings: { display: string; model: string }[] = [
    { display: 'Fall-Nr.', model: this.keys[0] },
    { display: 'Name, Vorname', model: this.keys[1] },
    { display: 'Aufnahmeart', model: this.keys[2] },
    { display: 'Arztbrieftyp', model: this.keys[3] },
    { display: 'Änderungsdatum', model: this.keys[4] },
    { display: 'Bearbeitungsstatus', model: this.keys[5] },
  ];
  perPageSize: any = 10;

  LetterDisplayedColumn: string[] = this.columnMappings.map(mapping => mapping.model);


  LetterDataSource = this.dataService.items;
}
