import {
  flexRender,
  type ColumnFiltersState,
  type SortingState,
  getCoreRowModel,
  useReactTable,
  getPaginationRowModel,
  getFilteredRowModel,
  getSortedRowModel,
  type ColumnDef,
} from "@tanstack/react-table";
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "../ui/table";
import { Button } from "../ui/button";
import { useState } from "react";
import { Input } from "../ui/input";
import clsx from "clsx";
import { DeleteDialog } from "../dialog/DeleteDialog";
import api from "../../utils/api";
import { handleApiError } from "../../utils/apiErrorHandler";
import { useNavigate } from "react-router";
import { toast } from "sonner";

interface DataTableProps<TData, TValue> {
  columns: ColumnDef<TData, TValue>[];
  data: TData[];
  apiRoute: string;
  filter: string;
}

export function DataTable<TData, TValue>({
  columns,
  data,
  apiRoute,
}: DataTableProps<TData, TValue>) {
  const [openDialog, setOpenDialog] = useState<boolean>(false);
  const [sorting, setSorting] = useState<SortingState>([]);
  const [globalFilter, setGlobalFilter] = useState<any>([]);
  const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([]);
  const [rowSelection, setRowSelection] = useState({});
  const navigate = useNavigate();

  const table = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    onSortingChange: setSorting,
    getSortedRowModel: getSortedRowModel(),
    onColumnFiltersChange: setColumnFilters,
    getFilteredRowModel: getFilteredRowModel(),
    onRowSelectionChange: setRowSelection,
    state: {
      sorting,
      columnFilters,
      globalFilter,
      rowSelection,
    },
  });

  const deleteMultiple = async () => {
    const selectedIndexes = Object.keys(rowSelection);
    const selectedIds = selectedIndexes.map(
      (index) => data[parseInt(index)].id
    );

    try {
      await api.delete(apiRoute, {
        data: { ids: selectedIds },
      });
      toast.success("Les éléments ont été supprimés avec succès.");
      navigate(0);
    } catch (error) {
      handleApiError(error);
    }
  };

  return (
    <>
      <div className="flex items-center py-4">
        <Input
          placeholder="Filtrer."
          value={globalFilter ?? ""}
          onChange={(e) => {
            setGlobalFilter(e.target.value);
            table.setGlobalFilter(String(e.target.value));
          }}
          className="max-w-sm"
        />
      </div>
      <div
        className={clsx(
          "transition-all duration-300 ease-in-out overflow-hidden",
          Object.keys(rowSelection).length === 0
            ? "opacity-0 max-h-0"
            : "opacity-100 max-h-20"
        )}
      >
        <Button
          type="button"
          className="bg-red-500 hover:bg-red-600 cursor-pointer"
          onClick={(e) => {
            e.stopPropagation();
            setOpenDialog(true);
          }}
        >
          Tout supprimer
        </Button>
      </div>
      <div className="rounded-md border">
        <Table>
          <TableCaption>Voici votre table de résultats.</TableCaption>
          <TableHeader>
            {table.getHeaderGroups().map((headerGroup) => (
              <TableRow key={headerGroup.id}>
                {headerGroup.headers.map((header) => {
                  return (
                    <TableHead key={header.id} className="text-white">
                      {header.isPlaceholder
                        ? null
                        : flexRender(
                            header.column.columnDef.header,
                            header.getContext()
                          )}
                    </TableHead>
                  );
                })}
              </TableRow>
            ))}
          </TableHeader>
          <TableBody>
            {table.getRowModel().rows?.length ? (
              table.getRowModel().rows.map((row) => (
                <TableRow
                  key={row.id}
                  data-state={row.getIsSelected() && "selected"}
                  style={{
                    backgroundColor: row.getIsSelected()
                      ? "#3f3f46"
                      : "#09090b",
                  }}
                >
                  {row.getVisibleCells().map((cell) => (
                    <TableCell key={cell.id}>
                      {flexRender(
                        cell.column.columnDef.cell,
                        cell.getContext()
                      )}
                    </TableCell>
                  ))}
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell
                  colSpan={columns.length}
                  className="h-24 text-center"
                >
                  Aucun résultats.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>
      <div className="flex w-[100px] items-center justify-center text-sm font-medium">
        Page {table.getState().pagination.pageIndex + 1} de{" "}
        {table.getPageCount()}
      </div>
      <div className="flex items-center justify-end space-x-2 py-4">
        <Button
          className="border cursor-pointer"
          onClick={() => table.previousPage()}
          disabled={!table.getCanPreviousPage()}
        >
          Précédent
        </Button>
        <Button
          className="border cursor-pointer"
          onClick={() => table.nextPage()}
          disabled={!table.getCanNextPage()}
        >
          Suivant
        </Button>
      </div>
      <DeleteDialog
        open={openDialog}
        message={
          "Souhaitez-vous vraiment supprimer les éléments sélectionnés ?"
        }
        onOpenChange={setOpenDialog}
        deleteFn={deleteMultiple}
      />
    </>
  );
}
