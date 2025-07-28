import { useEffect, useState } from "react";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from "../../ui/select";
import type { Image } from "../../../utils/type";
import { handleApiError } from "../../../utils/apiErrorHandler";
import api from "../../../utils/api";

interface ImageSelectorProps {
  onSelectedImage: (e?: Image) => void;
}

export default function ImageSelector({ onSelectedImage }: ImageSelectorProps) {
  const [images, setImages] = useState<Image[]>([]);

  const getUserImage = async () => {
    try {
      const response = await api.get("/api/upload");
      setImages(response.data);
    } catch (error) {
      handleApiError(error);
    }
  };

  useEffect(() => {
    getUserImage();
  }, []);

  const handleSelectChange = (e: any) => {
    let image = images.find((item) => item.id == e);
    onSelectedImage(image);
  };

  return (
    <Select onValueChange={handleSelectChange}>
      <SelectTrigger className="w-[180px]">
        <SelectValue placeholder="Choisir une image" />
      </SelectTrigger>
      <SelectContent>
        <SelectGroup>
          <SelectLabel>Images</SelectLabel>
          {images.map((img: Image, index) => (
            <SelectItem
              key={index}
              className="cursor-pointer"
              value={img.id.toString()}
            >
              {img.name}
            </SelectItem>
          ))}
        </SelectGroup>
      </SelectContent>
    </Select>
  );
}
