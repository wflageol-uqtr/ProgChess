import type { ColumnDef } from "@tanstack/react-table";
import { useNavigate } from "react-router";
import api from "../../utils/api";
import { toast } from "sonner";
import { Trash2 } from "lucide-react";

export const ImageColumns: ColumnDef<string>[] = [
  {
    header: "Image",
    cell: ({ row }) => {
      return (
        <img
          className="w-56 rounded shadow-md object-contain"
          src={`http://localhost:5290/Image/${row.original}`}
          alt={`Image - ${row.original}`}
          loading="lazy"
        />
      );
    },
  },
  {
    header: "Nom",
    accessorKey: "name",
    cell: ({ row }) => {
      return <div className="font-medium">{row.original}</div>;
    },
  },
  {
    header: "Actions",
    id: "actions",
    cell: ({ row }) => {
      const navigate = useNavigate();

      const handleDelete = async () => {
        try {
          const response = await api.delete(`/api/upload/${row.original}`);
          toast.success(response.data, {
            className: "bg-green-100",
          });
          navigate(0);
        } catch (error) {
          toast.error("Une erreur est survenue.");
        }
      };

      return (
        <button
          onClick={() => handleDelete()}
          aria-label="Supprimer l'image"
          className="cursor-pointer"
          title="Supprimer"
        >
          <Trash2 className="w-5 h-5 text-gray-500 hover:text-red-600 transition duration-300" />
        </button>
      );
    },
  },
];
