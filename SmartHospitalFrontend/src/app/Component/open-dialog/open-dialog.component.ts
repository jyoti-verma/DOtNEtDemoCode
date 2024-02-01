import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { LetterService } from '../../Services/letter.service';

@Component({
  selector: 'app-open-dialog',
  templateUrl: './open-dialog.component.html',
  styleUrls: ['./open-dialog.component.css'],
  encapsulation: ViewEncapsulation.None,

})
export class OpenDialogComponent {
  dialogHeading = `New Doctor's letter`;
  dialogText = `For which stay you would like to create a doctor's note?`;
  constructor(public dialogRef: MatDialogRef<OpenDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private dataService: LetterService) { }
  closeDialog(): void {
    this.dialogRef.close();
  }

  keys = Object.keys(this.dataService.items[0]);
  columnMappings: { display: string; model: string }[] = [
    { display: 'Fall-Nr.', model: this.keys[0] },
    { display: 'Aufenthalt am', model: this.keys[1] },
    { display: 'Aufnahmeart', model: this.keys[2] },
    { display: '', model: 'plus' },
  ];
  perPageSize: any = 6;

  NewLetterDisplayedColumn: string[] = this.columnMappings.map(mapping => mapping.model);

  NewLetterDataSource = this.dataService.items;
  

}
