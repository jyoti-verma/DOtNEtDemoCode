import { Component, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { OpenDialogComponent } from '../open-dialog/open-dialog.component';
import { LetterService } from '../../Services/letter.service';

@Component({
  selector: 'app-create-letter',
  templateUrl: './create-letter.component.html',
  styleUrls: ['./create-letter.component.css'],
  encapsulation: ViewEncapsulation.None,

})
export class CreateLetterComponent {
  NameOfLetter: string='';
  constructor(public dialog: MatDialog, private dataService: LetterService) { }

  openDialog(): void {
    let dialogRef = this.dialog.open(OpenDialogComponent, {
      width: '50%',
      height: '60%'
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }
  keys = Object.keys(this.dataService.items[0]);
  columnMappings: { display: string; model: string }[] = [
    { display: 'Fall-Nr.', model: this.keys[0] },
    { display: 'Name, Vorname', model: this.keys[1] },
    { display: 'Aufnahmeart', model: this.keys[2] },
  ];
  Title = "Neuen Arztbrief" + this.NameOfLetter;
  NewLetterDisplayedColumn: string[] = this.columnMappings.map(mapping => mapping.model);


  NewLetterDataSource = this.dataService.items;

}
