import React from 'react';

import { Swiper, SwiperSlide } from 'swiper/react';
import { Navigation, Pagination, Autoplay } from 'swiper/modules';
import {useNavigate} from 'react-router-dom';


import 'swiper/css';
import 'swiper/css/navigation';
import 'swiper/css/pagination';


interface MySweeperProps {
    appId: number[],
    ImageUrls: string[],

}


const MySweeper: React.FC<MySweeperProps> = ({ appId, ImageUrls }) => {


    const nev=useNavigate();


    return (
        <Swiper
            modules={[Navigation, Pagination, Autoplay]}
            slidesPerView={1}
            spaceBetween={20}
            navigation
            autoplay={{ delay: 3000 }}
            pagination={{ clickable: true }}
            breakpoints={{

                640: { slidesPerView: 2 },
                1024: { slidesPerView: 3 },
            }}
            className="mySwiper"
        >
            {appId.map((id, index) => {
                return(
                <SwiperSlide
                key={id}
                onClick={()=>
                    nev(`/game/${id}`,{state:{id:id}})
                }
                >
                    
                    <img
                        src={ImageUrls[index]}
                        alt="content"
                    />
                </SwiperSlide>

            )})}






        </Swiper>
    )

}
export default MySweeper;