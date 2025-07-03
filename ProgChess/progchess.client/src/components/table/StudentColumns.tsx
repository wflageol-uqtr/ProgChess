import type { ColumnDef } from "@tanstack/react-table";
import type { Student } from "../../utils/type";
import { useNavigate } from "react-router";
import api from "../../utils/api";
import { toast } from "sonner";
import { handleApiError } from "../../utils/apiErrorHandler";
import StudentAction from "./StudentAction";
import { Checkbox } from "../ui/checkbox";

export const StudentColumns: ColumnDef<Student>[] = [
  {
    id: "select",
    header: ({ table }) => (
      <Checkbox
        checked={
          table.getIsAllPageRowsSelected() ||
          (table.getIsSomePageRowsSelected() && "indeterminate")
        }
        onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
        aria-label="Select all"
      />
    ),
    cell: ({ row }) => (
      <Checkbox
        checked={row.getIsSelected()}
        onCheckedChange={(value) => row.toggleSelected(!!value)}
        aria-label="Select row"
      />
    ),
  },
  {
    accessorKey: "id",
    header: "Id",
  },
  {
    accessorKey: "permanentCode",
    header: "Code permanent",
  },
  {
    accessorKey: "createdAt",
    header: "Date de création",
  },
  {
    accessorKey: "updatedAt",
    header: "Date de modification",
  },
  {
    header: "Actions",
    id: "actions",
    cell: ({ row }) => {
      const navigate = useNavigate();

      const handleDelete = async () => {
        try {
          const response = await api.delete(
            `/api/student/delete/${row.getValue("id")}`
          );
          toast.success(response.data, {
            className: "bg-green-100",
          });
          navigate(0);
        } catch (error) {
          handleApiError(error);
        }
      };
      return <StudentAction id={row.getValue("id")} deleteFn={handleDelete} />;
    },
  },
];
